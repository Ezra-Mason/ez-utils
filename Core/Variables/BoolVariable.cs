using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    [CreateAssetMenu(menuName = "ez-utils/Runtime Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
    {
        [SerializeField] private bool _variable;
        public bool Value
        {
            get => _variable;
            set
            {
                _variable = value;
            }
        }
    }

}
