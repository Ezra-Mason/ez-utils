using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class WaitTask : TaskNode
    {
        private float _waitTime;
        private float _timer;

        protected override void OnStart()
        {
            _timer = 0;
        }

        protected override void OnStop()
        {

        }

        protected override NodeState OnUpdateNode(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer >= _waitTime) return NodeState.SUCCESS;

            return NodeState.RUNNING;
        }
    }
}
