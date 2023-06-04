using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class RootNode : Node
    {
        public override List<Node> Children => throw new System.NotImplementedException();
        public Node Child
        {
            get => _child;
            set
            {
                _child = value;
                //_child.SetParent(this);
            }
        }
        [SerializeField] protected Node _child;

        protected override void OnStart()
        { 
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdateNode(float deltaTime)
        {
            _child.UpdateNode(deltaTime);
            return NodeState.RUNNING;
        }
    }
}