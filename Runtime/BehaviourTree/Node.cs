using System.Collections;
using System.Collections.Generic;
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
    public abstract class Node
    {
        protected BehaviourTree _tree;
        public NodeState State => _state;
        protected NodeState _state;

        public Node Parent => _parent;
        protected Node _parent;

        protected bool _started = false;

        public Node(BehaviourTree tree)
        {
            _tree = tree;
        }

        //this might need to be changed to a set child node 
        public virtual void SetParent(Node node)
        {
            _parent = node;
        }


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
