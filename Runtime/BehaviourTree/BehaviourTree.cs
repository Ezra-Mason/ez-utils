using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;

        public bool SetRootNode(Node node)
        {
            if (_rootNode != null) return false;

            _rootNode = node;
            return true;
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
