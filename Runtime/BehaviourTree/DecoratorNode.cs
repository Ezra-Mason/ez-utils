using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    //TODO: change this from a decorator node to a node decorator
    /// <summary>
    /// Alters the execution of its single child node
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        public override List<Node> Children => new List<Node>() { _child };
        public Node Child 
        {
            get => _child;
            set
            {
                _child = value;
                //_child.SetParent(this);
            }
        }
        protected Node _child;
    }
}
