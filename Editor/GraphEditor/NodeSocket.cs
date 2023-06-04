using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ezutils.Editor
{
    public enum SocketType
    {
        IN,
        OUT
    }
    /// <summary>
    /// A connection point on a node for either an outgoing or an incomming connection 
    /// </summary>
    public class NodeSocket : IGraphElement, ISelectable
    {
        private SocketType _type;
        public GraphNode Node => _node;
        private GraphNode _node;
        private GUIStyle _style;
        public Rect Rect => _rect;

        public bool IsSelected { get; private set; }

        private Rect _rect;
        protected Action<NodeSocket> _onClicked;
        public NodeSocket(SocketType type, GraphNode node, GUIStyle style, Action<NodeSocket> onClicked)
        {
            _type = type;
            _node = node;
            _style = style;
            _onClicked = onClicked;
            _rect = new Rect(0f, 0f, 10, 20f);
        }

        // TODO: add functionality to change position of socket in implementing graph editor

        public void DrawElement()
        {
            _rect.y = _node.Rect.y + (_node.Rect.height * 0.5f) - _rect.height * 0.5f;

            switch (_type)
            {
                //TODO: remove magic numbers
                case SocketType.IN:
                    _rect.x = _node.Rect.x - Rect.width + 8f;
                    break;
                case SocketType.OUT:
                    _rect.x = _node.Rect.x + _node.Rect.width - 8f;
                    break;
                default:
                    break;
            }
            GUI.Label(_rect, $"{_type}");
            var clicked = GUI.Button(_rect, IsSelected ? "O" : "", _style);

            if (clicked && _onClicked != null)
            {
                _onClicked(this);
            }
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Deselct()
        {
            IsSelected = false;
        }
    }
}
