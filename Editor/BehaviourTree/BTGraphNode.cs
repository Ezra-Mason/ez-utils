using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ezutils.Runtime.BehaviourTree;
using ezutils.Core;

namespace ezutils.Editor
{

    public partial class BTGraphNode : GraphNode
    {
        protected string _nodeType = "NODE_TYPE";
        protected Vector2 _contentPadding = new Vector2(EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
        public BTGraphNode(Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(onClickIn, onClickOut, onClickRemove)
        {
            _defaultStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D
                },
                border = new RectOffset(12, 12, 12, 12)
            };

            _selectedStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D
                },
                border = new RectOffset(12, 12, 12, 12)
            };
            _activeStyle = _defaultStyle;
            _contentRect = new Rect(
                _contentPadding.x,
                _contentPadding.y,
                _rect.width - (_contentPadding.x *2f),
                _rect.height - (_contentPadding.y * 2f));

        }

        public override void DrawElement()
        {
            GUI.Box(_rect, "", _activeStyle);

            GUI.Label(_rect, Header, EditorStyles.largeLabel);

            InSocket?.DrawElement();
            OutSocket?.DrawElement();
        }


        protected override void ShowContextMenu()
        {
            base.ShowContextMenu();
        }

        public override void Move(Vector2 delta)
        {
            base.Move(delta);
            _contentRect.position += delta;
        }
    }
}
