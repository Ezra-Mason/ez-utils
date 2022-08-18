using System;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Runtime
{
    public class StackPool<T>
    {
        private readonly Stack<T> _pooledObjects;
        private readonly Func<T> _createInstance;
        private readonly Action<T> _onPut;
        private readonly Action<T> _onGet;

        public int Count => _pooledObjects.Count;
        public StackPool(Func<T> createInstance, Action<T> onGet = null, Action<T> onPut = null, int startSize = 0)
        {
            _pooledObjects = new Stack<T>();
            _createInstance = createInstance;
            _onGet = onGet;
            _onPut = onPut;
            for (int i = 0; i < startSize; i++)
            {
                Put(_createInstance.Invoke());
            }
        }

        public void Clear()
        {
            _pooledObjects.Clear();
        }

        public T Get()
        {
            T t = _pooledObjects.Count > 0 ? _pooledObjects.Pop() : _createInstance.Invoke();
            _onGet?.Invoke(t);

            return t;
        }

        public void Put(T instance)
        {
            _pooledObjects.Push(instance);
            _onPut?.Invoke(instance);
        }
    }
}