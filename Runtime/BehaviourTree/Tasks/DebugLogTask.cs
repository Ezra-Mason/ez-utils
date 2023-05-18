using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class DebugLogTask : TaskNode
    {
        protected string _log;

        protected override void OnStart()
        {
            Debug.Log($"OnStart: {_log}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop: {_log}");
        }

        protected override NodeState OnUpdateNode(float deltaTime)
        {
            Debug.Log($"OnUpdate: {_log}");
            return NodeState.SUCCESS;
        }
    }
}
