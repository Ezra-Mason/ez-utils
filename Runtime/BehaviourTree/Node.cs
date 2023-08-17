using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ezutils.Runtime.BehaviourTree
{
    public enum NodeState
    {
        RUNNING = 0x00,
        SUCCESS = 0x02,
        FAILURE = 0x04
    }

    /// <summary>
    /// Base class for any node within the BehaviourTree
    /// </summary>

    // I dont really want this to be an SO but it makes two things easier
    // - creating an instance from a type e.g. ScriptableObject.CreateInstance<T>()
    // - tracking nodes in a behaviour from the asset view
    public abstract class Node : ScriptableObject
    {
#if UNITY_EDITOR
        public Vector2 GraphPosition
        {
            get => _graphPosition; set
            {
                EditorUtility.SetDirty(this);
                _graphPosition = value;
            }
        }
#endif
        [SerializeField] protected Vector2 _graphPosition = new Vector2();

        public string NodeName { get; set; }

        [SerializeField] protected BehaviourTree _tree;
        public NodeState State => _state;
        [SerializeField] protected NodeState _state;

        public Node Parent => _parent;
        [SerializeField] protected Node _parent;
        public abstract List<Node> Children { get; }

        protected bool _started = false;
        /// <summary>
        /// Set this node to be part of the given tree
        /// </summary>
        /// <param name="tree"></param>
        /// <returns>was the node successfully set to part of the tree</returns>
        public bool SetTree(BehaviourTree tree)
        {
            if (_tree == null)
            {
                _tree = tree;
                return true;
            }
            return false;
        }

#if UNITY_EDITOR
        public void Save()
        {
            /*            AssetDatabase.Refresh();
                        EditorUtility.SetDirty(this);
            */
            AssetDatabase.SaveAssets();
        }

        public virtual void SetParent(Node node)
        {
            if (node == this)
            {
                Debug.LogError("Cannot parent a node to itself");
                return;
            }
            if (node == null)
            {
                Debug.Log($"nulling parent node of node {this.name}");
            }

            _parent = node;
            Save();
        }
#endif
        public virtual NodeState UpdateNode(float deltaTime)
        {
            // if this is the first tick, call the start method
            if (!_started)
            {
                OnStart();
                _started = true;
            }

            _state = OnUpdateNode(deltaTime);

            // if the node has started but has now exited, call the stop method
            var shouldExit = _started &&
                            (_state == NodeState.FAILURE || _state == NodeState.SUCCESS);
            if (shouldExit)
            {
                OnStop();
                _started = false;
            }

            return _state;
        }

        /// <summary>
        /// Called on the nodes first update
        /// </summary>
        protected abstract void OnStart();
        protected abstract NodeState OnUpdateNode(float deltaTime);
        /// <summary>
        /// Called on the nodes last update
        /// </summary>
        protected abstract void OnStop();


    }
}
