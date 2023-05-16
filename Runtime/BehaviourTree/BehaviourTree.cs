using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
#if UNITY_EDITOR

    public partial class BehaviourTree 
    {
        public Node EDITOR_RootNode => _rootNode;
    }

#endif
    [CreateAssetMenu(menuName ="ez-utils/Behaviour Tree")]
    public partial class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;
        [SerializeField] private List<Node> _nodes;

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
