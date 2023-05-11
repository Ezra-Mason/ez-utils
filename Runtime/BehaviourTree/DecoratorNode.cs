using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public abstract class DecoratorNode : Node
    {
        protected Node _child;

        protected DecoratorNode(BehaviourTree tree) : base(tree)
        {
        }

        public override void UpdateNode(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
