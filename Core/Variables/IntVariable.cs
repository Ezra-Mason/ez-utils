using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ez-utils/Runtime Variables/Int Variable")]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int _value;
    public int Value => _value;

    public void SetValue(int newValue)
    {
        _value = newValue;
    }
    public void Increment()
    {
        _value++;
    }
    public void Decrement()
    {
        _value--;
    }
}
