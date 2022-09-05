using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    /// <summary>
    /// Representation of a 1D spring
    /// </summary>
    [System.Serializable]
    public class Spring1D
    {
        public Spring1D(float value, float idealValue, float damping, float frequency)
        {
            _value = value;
            _idealValue = idealValue;
            _damping = damping;
            _frequency = frequency;
        }
        public float Value
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
        private float _value;
        public float IdealValue
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
        [SerializeField] private float _idealValue;
        public float Velocity
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
        private float _velocity;

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
