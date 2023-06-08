using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Editor
{
    public class DecoratorGraphNode : BTGraphNode
    {
        public DecoratorGraphNode(Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(onClickIn, onClickOut, onClickRemove)
        {
            _nodeType = "Decorator";
        }
    }
}