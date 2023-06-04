using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Editor
{

    public class CompositeGraphNode : BTGraphNode
    {
        public CompositeGraphNode(Vector2 position, GUIStyle inStyle, GUIStyle outStyle, Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(position, inStyle, outStyle, onClickIn, onClickOut, onClickRemove)
        {
            _nodeType = "Composite Node";
        }
    }

}