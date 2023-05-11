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

        public NodeState State => _rootNode.State;
    }
}
