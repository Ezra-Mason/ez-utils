using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ezutils.Editor
{

    public class TaskGraphNode : BTGraphNode
    {
        public TaskGraphNode(Action<NodeSocket> onClickIn, Action<NodeSocket> onClickOut, Action<GraphNode> onClickRemove) : base(onClickIn, onClickOut, onClickRemove)
        {
            OutSocket = null;
            _nodeType = "Task";
        }
    }
}