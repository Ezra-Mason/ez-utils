using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
#if UNITY_EDITOR
    using UnityEditor;

    public partial class BehaviourTree
    {
        public Node EDITOR_RootNode => _rootNode;
        public List<Node> EDITOR_Nodes => _nodes;

        /// <summary>
        /// Create a node of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(Type type) 
        {
            var node = ScriptableObject.CreateInstance(type) as Node;
            node.NodeName = type.Name;
            node.name = type.Name;
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

        public bool SetRootNode(Node node)
        {
            if (_rootNode != null) return false;

            _rootNode = node;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            return true;
        }

        public void Save()
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Save();
            }
        }
    }

#endif
    [CreateAssetMenu(menuName = "ez-utils/Behaviour Tree")]
    public partial class BehaviourTree : ScriptableObject
    {

        [SerializeField] private Node _rootNode;
        [SerializeField] private List<Node> _nodes = new List<Node>();
        
        public NodeState State => _rootNode.State;

        /// <summary>
        /// Link two <see cref="ezutils.Runtime.BehaviourTree.Node" />s together as child and parent
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        public void Connect(Node child, Node parent)
        {
            child.SetParent(parent);
            AddChild(child, parent);      
        }

        private void AddChild(Node child, Node parent)
        {
            var decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.Child = child;
            }
            var comp = parent as CompositeNode;
            if (comp)
            {
                comp.Children.Add(child);
            }
            var root = parent as RootNode;
            if (root)
            {
                root.Child = child;
            }
        }

        public NodeState UpdateTree(float deltaTime)
        {
            if (_rootNode.State == NodeState.RUNNING)
            {
                return _rootNode.UpdateNode(deltaTime);
            }
            return _rootNode.State;
        }
    }
}
