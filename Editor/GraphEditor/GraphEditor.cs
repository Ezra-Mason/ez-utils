using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ezutils.Editor
{
    public class GraphEditor : EditorWindow
    {
        Vector2 _mousePosition;

        protected List<GraphNode> _nodes;
        protected GUIStyle _nodeStyle;
        protected GUIStyle _nodeSelectedStyle;
        protected GenericMenu _addNodeMenu;

        //connections
        protected List<NodeConnection> _connection;
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

        private void OnEnable()
        {
            _isDirectional = true;
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);
            _nodeSelectedStyle = new GUIStyle();
            _nodeSelectedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
            _nodeSelectedStyle.border = new RectOffset(12, 12, 12, 12);

            _inSocketStyle = new GUIStyle();
            _inSocketStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            _inSocketStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            _inSocketStyle.border = new RectOffset(4, 4, 12, 12);

            _outSocketStyle = new GUIStyle();
            _outSocketStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            _outSocketStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            _outSocketStyle.border = new RectOffset(4, 4, 12, 12);

            _addNodeMenu = new GenericMenu();
            _addNodeMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(_mousePosition));

        }

        private void OnGUI()
        {

            DrawConnections();
            DrawNodes();

            ProcessNodeEvents();
            ProcessEvents();

            if (GUI.changed) Repaint();
        }
        private void DrawConnections()
        {
            if (_connection == null) return;
            if (_connection.Count == 0) return;

            for (int i = 0; i < _connection.Count; i++)
            {
                _connection[i].DrawElement();
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
        protected void ShowContextMenu()
        {

            _addNodeMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            if (_nodes == null)
            {
                _nodes = new List<GraphNode>();
            }

            _nodes.Add(new GraphNode(mousePosition, 200, 50, _nodeStyle,_nodeSelectedStyle, _inSocketStyle, _outSocketStyle, OnClickInSocket, OnClickOutSocket));
        }

        private void OnClickInSocket(NodeSocket socket)
        {
            _selectedInSocket = socket;
            socket.Select();
            Debug.Log("selected in node");
            if (_selectedOutSocket == null) return;
            if (_selectedOutSocket != _selectedInSocket)
            {
                Debug.Log("connected from in node");
                ConnectSelection();
            }
            DeselectSockets();
        }
        private void OnClickOutSocket(NodeSocket socket)
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

        protected void OnClickConnection(NodeConnection connection)
        {
            _connection.Remove(connection);
        }
        /// <summary>
        /// Create a connection between the selected sockets
        /// </summary>
        protected void ConnectSelection()
        {
            if (_connection == null)
            {
                _connection = new List<NodeConnection>();
            }

            _connection.Add(
                new NodeConnection(_selectedInSocket, _selectedOutSocket, OnClickConnection)
                );
            Debug.Log("connected node");

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
    }
}
