using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children 
        { 
            get => _children;
            set
            {
                _children = value;
                foreach(var child in _children)
                {
                    child.SetParent(this);
                }
            }
        }

        protected List<Node> _children = new List<Node>();
        protected CompositeNode(BehaviourTree tree) : base(tree)
        {
        }
    }
}
