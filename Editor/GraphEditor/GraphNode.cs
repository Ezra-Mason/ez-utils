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
        private Rect _rect;
        private GUIStyle _activeStyle;
        private readonly GUIStyle _defaultStyle;
        private readonly GUIStyle _selectedStyle;
        private string _title;
        private bool _beingDragged;

        //sockets
        public NodeSocket InSocket { get; set; }
        public NodeSocket OutSocket { get; set; }

        public bool IsSelected { get; private set; }

        private Action<NodeSocket> _onClickIn;
        private Action<NodeSocket> _onClickOut;
        public GraphNode(
            Vector2 position, 
            float width, 
            float height, 
            GUIStyle nodeStyle, 
            GUIStyle selectedStyle, 
            GUIStyle inStyle, 
            GUIStyle outStyle, 
            Action<NodeSocket> onClickIn, 
            Action<NodeSocket> onClickOut)
        {
            _rect = new Rect(position.x, position.y, width, height);
            _activeStyle = nodeStyle;
            _defaultStyle = nodeStyle;
            _selectedStyle = selectedStyle;
            InSocket = new NodeSocket(SocketType.IN, this, inStyle, onClickIn);
            OutSocket = new NodeSocket(SocketType.OUT, this, outStyle, onClickOut);
        }

        public void Move(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void DrawElement()
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
    }
}
