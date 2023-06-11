using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace ezutils.Editor
{
    public class RootGraphNode : BTGraphNode
    {
        public override GUIStyle Style
        {
            get
            {
                var style = GUI.skin.window;
                return style;
            }
            protected set
            {
                Style = value;
            }
        }
        public RootGraphNode(Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(onClickIn, onClickOut, onClickRemove)
        {
            Header = "Root";
            _nodeType = "Root Node";
            InSocket = null;
        }
    }
}