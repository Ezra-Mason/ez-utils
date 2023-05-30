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
        public BTGraphNode(Vector2 position, GUIStyle inStyle, GUIStyle outStyle, Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(position, inStyle, outStyle, onClickIn, onClickOut, onClickRemove)
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
        }

        public override void DrawElement()
        {
            InSocket.DrawElement();
            OutSocket.DrawElement();
            GUI.Box(_rect, _nodeType, _activeStyle);
            //GUI.Label(new Rect(_rect.x * 5, _rect.y * 5, _rect.width, _rect.height), _title);
        }


        protected override void ShowContextMenu()
        {
            base.ShowContextMenu();
        }
    }
}
