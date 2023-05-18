using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
#if UNITY_EDITOR

    public partial class BehaviourTree
    {
        public Node EDITOR_RootNode => _rootNode;
        public List<Node> EDITOR_Nodes => _nodes;
    }

#endif
    [CreateAssetMenu(menuName = "ez-utils/Behaviour Tree")]
    public partial class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;
        [SerializeField] private List<Node> _nodes = new List<Node>();

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

        /// <summary>
        /// Create a node of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(Type type) 
        {
            var node = ScriptableObject.CreateInstance(type) as Node;
            node.NodeName = type.Name;
            node.SetTree(this);
            Debug.Log($"Created a {node.NodeName} node");
            _nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// Delete the given node
        /// </summary>
        /// <param name="node">The node to be deleted</param>
        public void DeleteNode(Node node)
        {
            _nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
    }
}
