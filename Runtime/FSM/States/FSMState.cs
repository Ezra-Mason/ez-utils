using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.FSM
{
    public enum ExecutionState
    {
        NONE,
        ACTIVE,
        COMPLETE,
        TERMINATED
    }

    public abstract class FSMState : ScriptableObject
    {
        protected FiniteStateMachine _finiteStateMachine;
        public ExecutionState ExecutionState { get; protected set; }
        public bool HasEnteredState { get; protected set; }

        public virtual void OnEnable()
        {
            ExecutionState = ExecutionState.NONE;
        }

        /// <summary>
        /// Try to make this state the actively executing state
        /// </summary>
        /// <returns>Has the state been entered successfully</returns>
        public virtual bool EnterState()
        {
            ExecutionState = ExecutionState.ACTIVE;
            return true;
        }

        /// <summary>
        /// Process and update the behaviour of this state
        /// </summary>
        public abstract void UpdateState();

        /// <summary>
        /// Stop execiting this state as the active one.
        /// </summary>
        /// <returns>Has the state been exited successfully</returns>
        public virtual bool ExitState()
        {
            ExecutionState = ExecutionState.COMPLETE;
            return true;
        }
    }
}
