using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    [CreateAssetMenu(menuName = "ez-utils/Scriptable Var/Int")]
    public class IntVariable : ScriptableVar<int>
    {
        public static IntVariable operator ++(IntVariable i)
        {
            i.Value++;
            return i;
        }

        public static IntVariable operator --(IntVariable i)
        {
            i.Value--;
            return i;
        }
    }
}
