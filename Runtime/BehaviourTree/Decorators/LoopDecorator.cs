using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class LoopDecorator: DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdateNode(float deltaTime)
        {
            //TODO: make this loop for a given time, success count or indefinitely
            // this should execute the node and when it completes it resets the node and restarts the execution
            _child.UpdateNode(deltaTime);
            return NodeState.RUNNING;
        }
    }
}
