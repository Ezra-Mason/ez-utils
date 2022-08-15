using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ezutils.Core
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when event is raised.")]
        [SerializeField] private UnityEvent _response;
        public UnityEvent Response => _response;
        public bool ShouldLog => _shouldLog;
        [SerializeField] private bool _shouldLog;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            if (ShouldLog)
            {
                Debug.Log("Event " + Event + " raised");
            }
            Response.Invoke();
        }
    }
}
