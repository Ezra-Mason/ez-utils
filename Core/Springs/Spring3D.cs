using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    /// <summary>
    /// Representation of a 3D spring
    /// </summary>
    [System.Serializable]
    public class Spring3D
    {
        public Spring3D(Vector3 value, Vector3 idealValue, float damping, float frequency)
        {
            _value = value;
            _idealValue = idealValue;
            _damping = damping;
            _frequency = frequency;
        }
        public Vector3 Value
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
        private Vector3 _value;
        public float x { get => _value.x; set { _value.x = value; } }
        public float y { get => _value.y; set { _value.y = value; } }
        public float z { get => _value.z; set { _value.z = value; } }

        public Vector3 IdealValue
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
        [SerializeField] private Vector3 _idealValue;
        public Vector3 Velocity
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
        private Vector3 _velocity;
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
