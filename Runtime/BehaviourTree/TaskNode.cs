using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class TaskNode : Node
    {
        protected TaskNode(BehaviourTree tree) : base(tree)
        {
        }

    }
}
