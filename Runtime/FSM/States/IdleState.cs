using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.FSM
{
    [CreateAssetMenu(menuName ="ez-utils/FSM/States/Idle")]
    public class IdleState : FSMState
    {
        public override bool EnterState()
        {
            return base.EnterState();
        }

        public override void UpdateState()
        {
        }

        public override bool ExitState()
        {
            return base.ExitState();
        }
    }
}
