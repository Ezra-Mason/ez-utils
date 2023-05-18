using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    /// <summary>
    /// Executes child nodes until it reaches a one which fails
    /// </summary>
    public class SequenceComposite : CompositeNode
    {
        private int _executionIdx = 0;

        protected override void OnStart()
        {
            _executionIdx = 0;
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdateNode(float deltaTime)
        {
            var current = _children[_executionIdx];
            var state = current.UpdateNode(deltaTime);
            switch (state)
            {
                //this node is still running so we need to wait for it to complete
                case NodeState.RUNNING:
                    return state;
                // this child node is fine, move to the next in the list
                case NodeState.SUCCESS:
                    _executionIdx++;
                    break;
                // the child node causes this node to complete
                case NodeState.FAILURE:
                    return state;
                default:
                    break;
            }

            var completed = _children.Count == _executionIdx;
            return completed ? NodeState.SUCCESS : NodeState.RUNNING;
        }
    }
}
