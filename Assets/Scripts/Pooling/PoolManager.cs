using Blast.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blast.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] Transform _spawnParent;
        Dictionary<Type, IObjectPool> _pools;

        public void CreatePool<T>(T prefab, int spawnCap)
            where T : MonoBehaviour, IPoolable<T>
        {
            Type key = typeof(T);
            if(_pools.TryGetValue(key, out IObjectPool pool))
            {
                Debug.LogWarning($"Pool of type {key.Name} already exist.");
                return;
            }

            ObjectPool<T> newPool = new(prefab, spawnCap, _spawnParent);
            _pools.Add(key, newPool);
        }

        public T Spawn<T>(IData data)
            where T : MonoBehaviour, IPoolable<T>
        {
            Type key = typeof(T);
            if (!_pools.TryGetValue(key, out IObjectPool pool))
            {
                Debug.LogWarning($"No pool found for type {key.Name}.");
                return null;
            }

            ObjectPool<T> typedPool = (ObjectPool <T>)pool;
            return typedPool.Spawn(data);
        }

        private void Awake()
        {
            _pools = new Dictionary<Type, IObjectPool>();
        }
    }
}