using UnityEngine;
namespace ezutils.Core
{
    public abstract class ObjectRepository<T> : ScriptableObject
    {
        public T[] Repository => _repository;
        [SerializeField] private T[] _repository;

        public int Length => _repository.Length;
        public T this[int i]
        {
            get
            {
                return _repository[i];
            }
            set
            {
                _repository[i] = value;
            }
        }
    }
}
