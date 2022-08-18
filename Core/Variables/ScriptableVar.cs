using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{

    public abstract class ScriptableVar<T> : ScriptableObject
    {
        [SerializeField] private bool _readOnly;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_readOnly)
                {
                    Debug.LogError($"Trying to write to readonly ScriptableVar {this}");
                    return;
                }

                _value = value;
            }
        }
        [SerializeField] protected T _value;

        public static implicit operator T(ScriptableVar<T> t) => t.Value;
    }
}