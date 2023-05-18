using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ezutils.Runtime.BehaviourTree;
using System;

namespace ezutils.Editor
{

    public class BTGraphNode : GraphNode
    {
        protected string _nodeType = "NODE_TYPE";
        public BTGraphNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inStyle, GUIStyle outStyle, Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(position, width, height, nodeStyle, selectedStyle, inStyle, outStyle, onClickIn, onClickOut, onClickRemove)
        {
            
        }

        public override void DrawElement()
        {
            InSocket.DrawElement();
            OutSocket.DrawElement();
            GUI.Box(_rect, _nodeType, _activeStyle);
            var str = GUI.TextField(new Rect(_rect.x, _rect.y, _rect.width/3f ,_rect.height /3f), _title);
            if (str != _title)
            {
                _title = str;
            }
        }
    }
}
