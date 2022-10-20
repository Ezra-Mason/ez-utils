using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.FSM
{

    public class FiniteStateMachine : MonoBehaviour
    {
        [SerializeField] private FSMState _startingState;
        private FSMState _currentState;
        private void Awake()
        {
            _currentState = null;
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (_startingState != null)
            {
                EnterState(_startingState);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (_currentState == null) return;

            _currentState.UpdateState();
        }

        private void EnterState(FSMState targetState)
        {
            if (targetState != null) return;

            _currentState = targetState;
            _currentState.EnterState();

        }
    }
}
