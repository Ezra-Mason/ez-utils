using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> _children = new List<Node>();
        protected CompositeNode(BehaviourTree tree) : base(tree)
        {
        }
    }
}
