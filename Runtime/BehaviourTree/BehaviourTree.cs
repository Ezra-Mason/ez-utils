using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;

        public void TickTree(float deltaTime)
        {
            _rootNode.UpdateNode(deltaTime);
        }

        public NodeState UpdateTree(float deltaTime)
        {
            if (_rootNode.State == NodeState.RUNNING)
            {
                return _rootNode.UpdateNode(deltaTime);
            }

            return _rootNode.State;
        }

        public NodeState State => _rootNode.State;
    }
}
