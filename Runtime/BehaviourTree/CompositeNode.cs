using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        public override List<Node> Children 
        { 
            get => _children;
/*            protected set
            {
                _children = value;
                foreach(var child in _children)
                {
                    child.SetParent(this);
                }
            }*/
        }

        [SerializeField] protected List<Node> _children = new List<Node>();
    }
}
