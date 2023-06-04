using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class TaskNode : Node
    {
        public override List<Node> Children => null;
        //TODO: add generic parameter setting for task nodes
    }
}
