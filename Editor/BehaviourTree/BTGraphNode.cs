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
            _contentRect = new Rect(
                _contentPadding.x,
                _contentPadding.y,
                Rect.width - (_contentPadding.x * 2f),
                Rect.height - (_contentPadding.y * 2f));

        }

        public override void DrawElement()
        {
            base.DrawElement();
/*            //GUILayout.Box("", Style);

            GUILayout.Label(Header, EditorStyles.largeLabel);

            InSocket?.DrawElement();
            OutSocket?.DrawElement();*/
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
