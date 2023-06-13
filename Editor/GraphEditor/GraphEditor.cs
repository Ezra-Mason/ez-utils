using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine.UIElements;

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
        protected NodeSocket _selectedSourceSocket;
        protected NodeSocket _selectedTargetSocket;
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

            DrawNodes();
            DrawConnections();
            DrawDraggedConnection();

            GUILayout.Label(new GUIContent($" source: {_selectedSourceSocket?.Node.Header}, target: {_selectedTargetSocket?.Node.Header}"));
            //Context menu GUI

            // should remove
            //ProcessNodeEvents();

            ProcessEvents();

            //if (GUI.changed) Repaint();
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
        /// <summary>
        /// Draw the windows for all the nodes in the graph
        /// </summary>
        protected void DrawNodes()
        {
            BeginWindows();
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                node.Rect = GUILayout.Window(
                    i,
                    node.Rect,
                    delegate
                    {
                        OnNodeGUI(node);
                    },
                    node.Header,
                    node.Style
                    //,GUILayout.Width(0f), GUILayout.Height(0f)
                    );
            }
            EndWindows();
        }

        /// <summary>
        /// Draw the GUI content of the given <see cref="GraphNode"/> window
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OnNodeGUI(GraphNode node)
        {
            if (node.InSocket != null)
            {
                LayoutSocket(node.InSocket);
            }

            GUILayout.Label("node body");

            if (node.OutSocket != null)
            {
                LayoutSocket(node.OutSocket);
            }
            //drag nodes
            GUI.DragWindow();
        }

        /// <summary>
        /// Layout the GUI for the given <see cref="NodeSocket"/>
        /// </summary>
        /// <param name="socket"></param>
        private void LayoutSocket(NodeSocket socket)
        {
            int id = GUIUtility.GetControlID(FocusType.Passive);

            var style = socket.Type == SocketType.IN ? Styles.PinIn : Styles.PinOut;

            var label = new GUIContent(socket.Title);
            Rect rect = GUILayoutUtility.GetRect(label, style);

            if (Event.current.type != EventType.Layout && Event.current.type != EventType.Used)
            {
                socket.Rect = rect;

                bool clickedSocket = rect.Contains(Event.current.mousePosition)
                                    && Event.current.button == 0;

                switch (Event.current.GetTypeForControl(id))
                {
                    // begin dragging on a socket
                    case EventType.MouseDown:
                        if (clickedSocket)
                        {
                            Debug.Log("clicked on socket");

                            if (true)
                            {
                                _selectedSourceSocket = socket;
                            }
                            Event.current.Use();
                        }
                        break;
                    case EventType.MouseUp:
                        if (clickedSocket)
                        {
                            //end drag
                            if (_selectedTargetSocket == socket)
                            {
                                ConnectSelection();
                                _selectedTargetSocket = (_selectedSourceSocket = null);
                            }
                            Event.current.Use();
                        }
                        break;
                    case EventType.MouseDrag:
                        //drag 
                        if (clickedSocket)
                        {
                            if (_selectedSourceSocket == null || _selectedSourceSocket == socket) return;

                            _selectedTargetSocket = socket;
                            Event.current.Use();
                        }
                        break;
                    case EventType.Repaint:
                        style.Draw(rect, label, id, socket.IsConnected);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DrawConnections()
        {
            if (Event.current.type == EventType.Repaint)
            {
                if (_connections == null) return;
                if (_connections.Count == 0) return;

                for (int i = 0; i < _connections.Count; i++)
                {
                    DrawConnection(_connections[i]);
                }
            }
        }

        private void DrawConnection(NodeConnection connection)
        {
            if (Event.current.type == EventType.Repaint)
            {
                var start = new Vector2(connection.Out.Node.Rect.xMax, connection.Out.Node.Rect.y + connection.Out.Rect.y + 9f);
                var end = new Vector2(connection.In.Node.Rect.x, connection.In.Node.Rect.y + connection.In.Rect.y + 9f);

                Handles.DrawBezier(
                    start,
                    end,
                    start - Vector2.left * 10f,
                    end + Vector2.left * 10f,
                    Color.white,
                    null,
                    2f);
            }

            if (_selectedSourceSocket != null && Event.current.type == EventType.MouseUp)
            {

                _selectedSourceSocket = (_selectedTargetSocket = null);
                Event.current.Use();
            }
        }

        private void DrawDraggedConnection()
        {
            if (_selectedSourceSocket == null) return;

            switch (Event.current.GetTypeForControl(0))
            {
                case EventType.Repaint:
                    var rect = _selectedSourceSocket.Rect;
                    rect.position += _selectedSourceSocket.Node.Rect.position;
                    var end = Event.current.mousePosition;

                    if (_selectedTargetSocket != null)
                    {
                        var rect2 = _selectedTargetSocket.Rect;
                        rect2.position += _selectedTargetSocket.Node.Rect.position;
                        end = new Vector2(rect2.x, rect2.y + 9f);
                    }

                    var start = new Vector2(
                                _selectedSourceSocket.Type == SocketType.OUT ? rect.xMax : rect.x,
                                rect.y + 9f);

                    Handles.DrawBezier(
                        start,
                        end,
                        start - Vector2.left * 10f,
                        end + Vector2.left * 10f,
                        Color.white,
                        null,
                        2f);
                    break;
                case EventType.MouseDrag:
                    _selectedTargetSocket = null;
                    Event.current.Use();
                    break;
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
                    if (Event.current.button == 0 && _selectedSourceSocket != null)
                    {
                        DeselectSockets();
                    }
                    break;
                case EventType.MouseUp:
                    if (Event.current.button == 0 && _selectedSourceSocket != null)
                    {
                        DeselectSockets();
                        Event.current.Use();
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
            /*            //_selectedTargetSocket = socket;
                        socket.Select();
                        Debug.Log("selected in node");
                        if (_selectedSourceSocket == null) return;
                        if (_selectedSourceSocket != _selectedTargetSocket)
                        {
                            Debug.Log($"connected {_selectedSourceSocket.Node}in to {_selectedTargetSocket.Node} ");
                            ConnectSelection();
                        }
                        DeselectSockets();*/
        }
        protected virtual void OnClickOutSocket(NodeSocket socket)
        {
            _selectedSourceSocket = socket;
            socket.Select();
            Debug.Log("selected out node");
            if (_selectedTargetSocket == null) return;
            if (_selectedSourceSocket != _selectedTargetSocket && !_isDirectional)
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
                new NodeConnection(_selectedTargetSocket, _selectedSourceSocket, OnClickConnection)
                );
            _selectedTargetSocket.Connect();
            _selectedSourceSocket.Connect();
            Debug.Log($"created connection from {_selectedSourceSocket.Node} to {_selectedTargetSocket.Node} node");

        }
        /// <summary>
        /// Null all socket selections
        /// </summary>
        protected void DeselectSockets()
        {
            _selectedTargetSocket?.Deselct();
            _selectedSourceSocket?.Deselct();
            _selectedTargetSocket = null;
            _selectedSourceSocket = null;
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
