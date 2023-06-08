using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Editor
{

    public class SequenceGraphNode : BTGraphNode
    {
        public SequenceGraphNode(Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(onClickIn, onClickOut, onClickRemove)
        {
            _nodeType = "Sequence";
        }
    }

}