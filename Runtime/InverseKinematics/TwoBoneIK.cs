using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime.IK
{
    public class TwoBoneIK : MonoBehaviour
    {
        private const int CHAIN_LENGTH = 3;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _tip;
        [SerializeField] private Transform _pole;

        [SerializeField] private int _iterations = 10;
        [SerializeField] private float _delta = 0.001f;


        [SerializeField] private Transform[] _bones;
        private Vector3[] _positions;
        private float[] _boneLengths;
        [SerializeField] private float _completeLength;

        private Vector3[] _initialDirection;
        private Quaternion[] _initalRots;
        private Quaternion _targetInitialRot;
        private Quaternion _rootInitialRot;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Initialise the arrays of the bones and lengths
        /// </summary>
        private void Init()
        {
            _positions = new Vector3[CHAIN_LENGTH + 1];
            _boneLengths = new float[CHAIN_LENGTH];
            _completeLength = 0f;

            //rotations
            _initialDirection = new Vector3[CHAIN_LENGTH + 1];
            _initalRots = new Quaternion[CHAIN_LENGTH + 1];

            _targetInitialRot = _target.rotation;

            for (int i = _bones.Length - 1; i >= 0; i--)
            {
                _initalRots[i] = _bones[i].rotation;
                if (i == _bones.Length - 1)
                {
                    _initialDirection[i] = _target.position - _bones[i].position;
                }
                else
                {
                    _initialDirection[i] = _bones[i + 1].position - _bones[i].position;
                    _boneLengths[i] = (_bones[i + 1].position - _bones[i].position).magnitude;
                    _completeLength += _boneLengths[i];
                }
            }
        }

        private void LateUpdate()
        {
            ResolveIK();
        }

        /// <summary>
        /// Resolve bone positions once using the FABRIK algorithm 
        /// </summary>
        private void ResolveIK()
        {
            if (_target == null)
                return;

            if (_boneLengths.Length != CHAIN_LENGTH)
                Init();

            for (int i = 0; i < _bones.Length; i++)
            {
                _positions[i] = _bones[i].position;
            }

            Quaternion rootRot = _bones[0].parent != null ? _bones[0].parent.rotation : Quaternion.identity;
            Quaternion rootRotDiff = rootRot * Quaternion.Inverse(_rootInitialRot);

            bool outOfReach = (_target.position - _bones[0].position).sqrMagnitude >= _completeLength * _completeLength;
            if (outOfReach)
            {
                Vector3 direction = (_target.position - _positions[0]).normalized;
                for (int i = 1; i < _positions.Length; i++)
                {
                    _positions[i] = _positions[i - 1] + direction * _boneLengths[i - 1];
                }
            }
            else
            {
                for (int i = 0; i < _positions.Length - 1; i++)
                {
                    _positions[i + 1] = Vector3.Lerp(_positions[i + 1], _positions[i] + rootRotDiff * _initialDirection[i], 5f);
                }
                for (int i = 0; i < _iterations; i++)
                {
                    //backwards pass
                    for (int j = _positions.Length - 1; j > 0; j--)
                    {
                        if (j == _positions.Length - 1)
                            _positions[j] = _target.position;
                        else
                            _positions[j] = _positions[j + 1] + (_positions[j] - _positions[j + 1]).normalized * _boneLengths[j];
                    }


                    //forward pass
                    for (int j = 1; j < _positions.Length; j++)
                    {
                        _positions[j] = _positions[j - 1] + (_positions[j] - _positions[j - 1]).normalized * _boneLengths[j - 1];
                    }

                    //if close enough stop
                    bool isCloseEnough = (_positions[_positions.Length - 1] - _target.position).sqrMagnitude < _delta * _delta;

                    if (isCloseEnough) break;
                }
            }


            if (_pole != null)
            {
                for (int i = 1; i < _positions.Length - 1; i++)
                {
                    Plane plane = new Plane(_positions[i + 1] - _positions[i - 1], _positions[i - 1]);
                    Vector3 poleProjection = plane.ClosestPointOnPlane(_pole.position);
                    Vector3 boneProjection = plane.ClosestPointOnPlane(_positions[i]);
                    float angle = Vector3.SignedAngle(boneProjection - _positions[i - 1], poleProjection - _positions[i - 1], plane.normal);
                    _positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (_positions[i] - _positions[i - 1]) + _positions[i - 1];
                }
            }

            for (int i = 0; i <= _bones.Length - 1; i++)
            {

                if (i == _positions.Length - 1)
                {
                    _bones[i].rotation = _target.rotation * Quaternion.Inverse(_targetInitialRot) * _initalRots[i];
                }
                else
                {
                    _bones[i].rotation = Quaternion.FromToRotation(_bones[i].InverseTransformDirection(_bones[i].up).normalized, (_positions[i + 1] - _positions[i]).normalized);
                }
                _bones[i].position = _positions[i];
            }
        }
    }
}