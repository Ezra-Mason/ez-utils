using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        private GUIStyle _connectedStyle;
        public Rect Rect => _rect;

        public bool IsSelected { get; private set; }
        public bool IsConnected { get; private set; }

        private Rect _rect;
        protected Action<NodeSocket> _onClicked;
        public NodeSocket(SocketType type, GraphNode node, Action<NodeSocket> onClicked)
        {
            _type = type;
            _node = node;
            _style = new GUIStyle
            {
                normal =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/radio.png") as Texture2D,
            },
                active =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/radio focus.png") as Texture2D
            }
            };
            _connectedStyle = new GUIStyle
            {
                normal =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/radio on.png") as Texture2D,
            },
                active =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/radio on focus.png") as Texture2D
            }
            };
            _onClicked = onClicked;
            IsConnected = false;
            _rect = new Rect(0f, 0f, 14f, 14f);
        }

        // TODO: add functionality to change position of socket in implementing graph editor

        public void DrawElement()
        {
            _rect.y = _node.Rect.y + (_node.Rect.height * 0.5f) - _rect.height * 0.5f;

            switch (_type)
            {
                //TODO: remove magic numbers
                case SocketType.IN:
                    _rect.x = _node.Rect.center.x - _node.Rect.width * 0.5f;// + (EditorGUIUtility.singleLineHeight * 0.5f);
                    break;
                case SocketType.OUT:
                    _rect.x = (_node.Rect.x + _node.Rect.width) - Rect.width;
                    break;
                default:
                    break;
            }
            var clicked = GUI.Button(_rect, "", IsConnected ? _connectedStyle : _style);

            if (clicked && _onClicked != null)
            {
                _onClicked(this);
            }
        }

        public void Connect()
        {
            IsConnected = true;
        }
        public void Disconnect()
        {
            IsConnected = false;
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
