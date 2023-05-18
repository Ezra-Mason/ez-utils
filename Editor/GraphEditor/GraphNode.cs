using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel.Design.Serialization;

namespace ezutils.Editor
{
    public class GraphNode : IGraphElement, ISelectable
    {
        public Rect Rect => _rect;
        protected Rect _rect;
        protected GUIStyle _activeStyle;
        protected readonly GUIStyle _defaultStyle;
        protected readonly GUIStyle _selectedStyle;
        protected string _title;
        protected bool _beingDragged;

        //sockets
        public NodeSocket InSocket { get; set; }
        public NodeSocket OutSocket { get; set; }

        public bool IsSelected { get; private set; }

        private Action<NodeSocket> _onClickIn;
        private Action<NodeSocket> _onClickOut;
        private Action<GraphNode> _onRemove;

        private GenericMenu _contextMenu;
        public GraphNode(
            Vector2 position, 
            float width, 
            float height, 
            GUIStyle nodeStyle, 
            GUIStyle selectedStyle, 
            GUIStyle inStyle, 
            GUIStyle outStyle, 
            Action<NodeSocket> onClickIn, 
            Action<NodeSocket> onClickOut,
            Action<GraphNode> onClickRemove)
        {
            _rect = new Rect(position.x, position.y, width, height);
            _activeStyle = nodeStyle;
            _defaultStyle = nodeStyle;
            _selectedStyle = selectedStyle;
            InSocket = new NodeSocket(SocketType.IN, this, inStyle, onClickIn);
            OutSocket = new NodeSocket(SocketType.OUT, this, outStyle, onClickOut);
            _onRemove = onClickRemove;

            _contextMenu = new GenericMenu();
            _contextMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemove);

        }

        public void Move(Vector2 delta)
        {
            _rect.position += delta;
        }

        public virtual void DrawElement()
        {
            InSocket.DrawElement();
            OutSocket.DrawElement();
            GUI.Box(_rect, _title, _activeStyle);
        }
        /// <summary>
        /// Process input events
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Should the GUI be repainted</returns>
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    //lmb
                    if (e.button == 0)
                    {

                        if (_rect.Contains(e.mousePosition))
                        {
                            _beingDragged = true;
                            Select();
                        }
                        else
                        {
                            Deselct();
                        }
                        GUI.changed = true;
                    }
                    //rmb
                    if (e.button == 1 && IsSelected && _rect.Contains(e.mousePosition))
                    {
                        ShowContextMenu();
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    _beingDragged = false;
                    break;

                case EventType.MouseDrag:
                    //when dragging the mouse move this node and use the event to prevent its use by others
                    if (e.button == 0 && _beingDragged)
                    {
                        Move(e.delta);
                        e.Use();
                        // the gui should be repainted now that the node has moved
                        return true;
                    }
                    break;

                default:
                    break;
            }
            return false;
        }

        public void Select()
        {
            IsSelected = true;
            _activeStyle = _selectedStyle;
        }

        public void Deselct()
        {
            IsSelected = true;
            _activeStyle = _defaultStyle;
        }
        private void ShowContextMenu()
        {
            _contextMenu.ShowAsContext();
        }
        private void OnClickRemove()
        {
            if (_onRemove == null) return;
            Debug.Log($"onclickRemove");
            _onRemove(this);
        }
    }
}
