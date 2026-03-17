using Blast.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        Dictionary<Type, IObjectPool> _pools;

        public async Task CreatePool<T>(T prefab, int spawnCap, Action<T> returnAction, 
            Transform activeParent, Transform inactiveParent = null) 
            where T : MonoBehaviour, IPoolable<T>
        {
            Type key = typeof(T);
            if(_pools.TryGetValue(key, out IObjectPool pool))
            {
                Debug.LogWarning($"Pool of type {key.Name} already exist.");
                return;
            }

            ObjectPool<T> newPool = new(prefab, spawnCap, returnAction, activeParent, inactiveParent);
            _pools.Add(key, newPool);
        }

        public T Spawn<T>(ISpawnData data)
            where T : MonoBehaviour, IPoolable<T>
        {
            Type key = typeof(T);
            if (!_pools.TryGetValue(key, out IObjectPool pool))
            {
                Debug.LogWarning($"No pool found for type {key.Name}. A new pool was created.");
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