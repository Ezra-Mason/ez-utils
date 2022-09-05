using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    /// <summary>
    /// Representation of a 2D spring
    /// </summary>
    [System.Serializable]
    public class Spring2D
    {
        public Spring2D(Vector2 value, Vector2 idealValue, float damping, float frequency)
        {
            _value = value;
            _idealValue = idealValue;
            _damping = damping;
            _frequency = frequency;
        }
        public Vector2 Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        private Vector2 _value;
        public Vector2 IdealValue
        {
            get
            {
                return _idealValue;
            }
            set
            {
                _idealValue = value;
            }
        }
        [SerializeField] private Vector2 _idealValue;
        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }
        private Vector2 _velocity;
        public float Damping
        {
            get
            {
                return _damping;
            }
            set
            {
                _damping = value;
            }
        }
        [SerializeField] private float _damping;
        public float Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
            }
        }
        [SerializeField] private float _frequency;
        /// <summary>
        /// Update the motion of the spring for the given change in time
        /// </summary>
        /// <param name="deltaTime">time to update over</param>
        public void Update(float deltaTime)
        {
            Springs.UpdateDampedSHMFast(ref _value, ref _velocity, _idealValue, deltaTime, _frequency, _damping);
        }
    }
}
