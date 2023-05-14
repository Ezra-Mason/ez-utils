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
        protected GenericMenu _addNodeMenu;

        [MenuItem("ez-utils/Graph Editor")]
        private static void OpenWindow()
        {
            GraphEditor window = GetWindow<GraphEditor>();
            window.titleContent = new GUIContent("Graph view");
        }

        private void OnEnable()
        {
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);

            _addNodeMenu = new GenericMenu();
            _addNodeMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(_mousePosition));

        }

        private void OnGUI()
        {
            DrawNodes();

            ProcessEvents();

            if (GUI.changed) Repaint();
        }

        private void DrawNodes()
        {
            if (_nodes == null) return;
            if (_nodes.Count == 0) return;

            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Draw();
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
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
                    break;
                case EventType.MouseDrag:
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    break;
                case EventType.ScrollWheel:
                    break;
                case EventType.Repaint:
                    break;
                case EventType.Layout:
                    break;
                case EventType.DragUpdated:
                    break;
                case EventType.DragPerform:
                    break;
                case EventType.DragExited:
                    break;
                case EventType.Ignore:
                    break;
                case EventType.Used:
                    break;
                case EventType.ValidateCommand:
                    break;
                case EventType.ExecuteCommand:
                    break;
                case EventType.ContextClick:
                    break;
                case EventType.MouseEnterWindow:
                    break;
                case EventType.MouseLeaveWindow:
                    break;
                default:
                    break;
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

            _nodes.Add(new GraphNode(mousePosition, 200, 50, _nodeStyle));
        }
    }
}
