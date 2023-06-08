using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ezutils.Editor
{
    public class GraphEditor : EditorWindow
    {
        protected Vector2 _mousePosition;
        Vector2 _delta;
        Vector2 _offset;

        protected List<GraphNode> _nodes;
        protected GenericMenu _addNodeMenu;

        //connections
        protected List<NodeConnection> _connections = new List<NodeConnection>();
        protected GUIStyle _inSocketStyle;
        protected GUIStyle _outSocketStyle;
        protected NodeSocket _selectedInSocket;
        protected NodeSocket _selectedOutSocket;
        protected bool _isDirectional = false;

        [MenuItem("ez-utils/Graph Editor")]
        private static void OpenWindow()
        {
            GraphEditor window = GetWindow<GraphEditor>();
            window.titleContent = new GUIContent("Graph view");
        }

        protected virtual void OnEnable()
        {
            _isDirectional = true;


            _addNodeMenu = new GenericMenu();
            _addNodeMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(_mousePosition));
            if (_nodes == null)
            {
                _nodes = new List<GraphNode>();
            }
        }
        protected virtual void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);
            DrawConnections();
            DrawNodes();

            ProcessNodeEvents();
            ProcessEvents();

            if (GUI.changed) Repaint();
        }
        private void DrawGrid(float spacing, float opacity, Color color)
        {
            int x = Mathf.CeilToInt(position.width / spacing);
            int y = Mathf.CeilToInt(position.height / spacing);

            Handles.BeginGUI();
            using (new Handles.DrawingScope(color: new Color(color.r, color.g, color.b, opacity)))
            {

                Vector3 offset = new Vector3(_offset.x % spacing, _offset.y % spacing, 0);
                // vertical lines
                for (int i = 0; i < x; i++)
                {
                    Handles.DrawLine(
                        new Vector3(spacing * i, -spacing, 0f) + offset,
                        new Vector3(spacing * i, position.height + spacing, 0f) + offset);
                }
                // horizontal lines
                for (int i = 0; i < y; i++)
                {
                    Handles.DrawLine(
                        new Vector3(-spacing, spacing * i, 0f) + offset,
                        new Vector3(position.width + spacing, spacing * i, 0f) + offset);
                }
            }
            Handles.EndGUI();
        }
        private void DrawConnections()
        {
            if (_connections == null) return;
            if (_connections.Count == 0) return;

            for (int i = 0; i < _connections.Count; i++)
            {
                _connections[i].DrawElement();
            }
        }

        private void DrawNodes()
        {
            if (_nodes == null) return;
            if (_nodes.Count == 0) return;

            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].DrawElement();
            }
        }
        private void ProcessEvents()
        {
            _mousePosition = Event.current.mousePosition;
            _delta = Vector2.zero;
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (Event.current.button == 1)
                    {
                        ShowContextMenu();
                    }
                    if (Event.current.button == 0 && _selectedOutSocket != null)
                    {
                        DeselectSockets();
                    }
                    break;
                case EventType.MouseDrag:
                    if (Event.current.button == 2)
                    {
                        OnDrag(Event.current.delta);
                    }
                    break;
                default:
                    break;
            }
        }
        protected void ProcessNodeEvents()
        {
            if (_nodes == null) return;

            // go backwards as we drew the nodes forwards, this makes the last node
            // the one drawn at the top so process its events first
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                var guiChanged = _nodes[i].ProcessEvents(Event.current);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
        protected virtual void ShowContextMenu()
        {

            _addNodeMenu.ShowAsContext();
        }
        private void OnClickAddNode(Vector2 mousePosition)
        {
            if (_nodes == null)
            {
                _nodes = new List<GraphNode>();
            }

            _nodes.Add(
                new GraphNode(
                    OnClickInSocket,
                    OnClickOutSocket,
                    OnClickRemove
                    )
                );
            _nodes[_nodes.Count - 1].Move(mousePosition);
        }

        #region Connections
        protected void OnClickInSocket(NodeSocket socket)
        {
            _selectedInSocket = socket;
            socket.Select();
            Debug.Log("selected in node");
            if (_selectedOutSocket == null) return;
            if (_selectedOutSocket != _selectedInSocket)
            {
                Debug.Log($"connected {_selectedOutSocket.Node}in to {_selectedInSocket.Node} ");
                ConnectSelection();
            }
            DeselectSockets();
        }
        protected virtual void OnClickOutSocket(NodeSocket socket)
        {
            _selectedOutSocket = socket;
            socket.Select();
            Debug.Log("selected out node");
            if (_selectedInSocket == null) return;
            if (_selectedOutSocket != _selectedInSocket && !_isDirectional)
            {
                ConnectSelection();
                Debug.Log("connected from out node");
            }

            DeselectSockets();
        }
        protected virtual void OnClickConnection(NodeConnection connection)
        {
            connection.In.Disconnect();
            connection.Out.Disconnect();
            _connections.Remove(connection);

        }
        #endregion

        /// <summary>
        /// Process the users removal of graph node
        /// </summary>
        protected virtual void OnClickRemove(GraphNode node)
        {
            // remove any connections which involve this node first
            if (_connections != null)
            {
                var toRemove = new List<NodeConnection>();
                for (int i = _connections.Count - 1; i >= 0; i--)
                {
                    if (_connections[i].In == node.InSocket || _connections[i].Out == node.OutSocket)
                    {
                        _connections.RemoveAt(i);
                    }
                }
            }
            _nodes.Remove(node);
        }

        /// <summary>
        /// Create a connection between the selected sockets
        /// </summary>
        protected virtual void ConnectSelection()
        {
            if (_connections == null)
            {
                _connections = new List<NodeConnection>();
            }

            _connections.Add(
                new NodeConnection(_selectedInSocket, _selectedOutSocket, OnClickConnection)
                );
            _selectedInSocket.Connect();
            _selectedOutSocket.Connect();
            Debug.Log($"created connection from {_selectedOutSocket.Node} to {_selectedInSocket.Node} node");

        }
        /// <summary>
        /// Null all socket selections
        /// </summary>
        protected void DeselectSockets()
        {
            _selectedInSocket?.Deselct();
            _selectedOutSocket?.Deselct();
            _selectedInSocket = null;
            _selectedOutSocket = null;
        }
        protected void OnDrag(Vector2 delta)
        {
            _delta = delta;
            _offset += _delta;

            if (_nodes == null) return;

            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Move(delta);
            }

            GUI.changed = true;
        }
    }
}
