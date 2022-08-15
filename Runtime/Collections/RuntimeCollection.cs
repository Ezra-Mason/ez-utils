using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RuntimeCollection<T> : ScriptableObject
{
    [SerializeField] private bool _allowDuplicates = true;
    public List<T> Items => _items;
    [SerializeField] protected List<T> _items = new List<T>();
    public int Count => _items.Count;

    public T this[int i]
    {
        get => _items[i];
        set => _items[i] = value;
    }

    private void OnEnable()
    {
        _items = new List<T>();
    }
    private void OnDisable()
    {
        _items.Clear();
    }
    public void Add(T t)
    {
        if (_items.Contains(t) && !_allowDuplicates)
        {
            Debug.LogError($"Attempting to add duplicate of {t} when duplicates are disabled on {name}");
        }
        else
        {
            _items.Add(t);
        }
    }

    public void Remove(T t)
    {
        if (_items.Contains(t))
            _items.Remove(t);
    }

    public void Clear()
    {
        _items.Clear();
    }
}
