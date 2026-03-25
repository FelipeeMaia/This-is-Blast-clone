using Blast.Data;
using Blast.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Pooling
{
    /// <summary>
    /// Class responsible to manage all the ObjectPools, and through which all calls to them are made.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] PoolData[] _poolsToCreate;
        Dictionary<Type, ObjectPool> _pools;

        public async Task CreatePool(PooledObject prefab, int spawnCap, 
            Transform poolParent, Action<PooledObject> returnAction = null)
        {
            Type poolType = prefab.GetType();
            if (_pools.TryGetValue(poolType, out ObjectPool pool))
            {
                Debug.LogWarning($"Pool of type {poolType.Name} already exist.");
                return;
            }

            ObjectPool newPool = new(prefab, spawnCap, poolParent, returnAction);
            _pools.Add(poolType, newPool);
        }

        public T Spawn<T>(ISpawnData data) where T : PooledObject
        {
            Type key = typeof(T);
            if (!_pools.TryGetValue(key, out ObjectPool pool))
            {
                Debug.LogWarning($"No pool found for type {key.Name}. A new pool was created.");
                return null;
            }

            var obj = (T)pool.Spawn(data);
            return obj;
        }

        private async void CreatePools()
        {
            foreach (var data in _poolsToCreate)
            {
                if (data.prefab is not PooledObject prefab) return;
                Type poolType = data.prefab.GetType();

                var poolParent = new GameObject($"{poolType.Name}'s Pool").transform;
                poolParent.position = Vector3.zero;
                poolParent.parent = transform;

                await CreatePool(prefab, data.initialAmmount, poolParent);
            }
        }
        
        private void Awake()
        {
            _pools = new Dictionary<Type, ObjectPool>();

            CreatePools();
        }
    }
}