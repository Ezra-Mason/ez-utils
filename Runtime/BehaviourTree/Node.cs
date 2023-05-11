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

        public virtual void UpdateNode(float deltaTime)
        {
            // if this is the first tick, call the start method
            if (!_started)
            {
                OnStart();
                _started = true;
            }

            OnUpdateNode(deltaTime);

            // if the node has started but has now exited, call the stop method
            var shouldExit = _started && 
                            (_state == NodeState.FAILURE || _state == NodeState.SUCCESS);
            if (shouldExit)
            {
                OnStop();
                _started = false;
            }
        }

        /// <summary>
        /// Called on the nodes first update
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdateNode(float deltaTime);
        /// <summary>
        /// Called on the nodes last update
        /// </summary>
        protected abstract void OnStop();
    }
}
