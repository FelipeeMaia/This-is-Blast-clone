using Blast.Interfaces;
using System;
using UnityEngine;

namespace Blast.Pooling
{
    public abstract class PooledObject : MonoBehaviour
    {
        public abstract void OnSpawn(ISpawnData spawnData);
        public virtual void ReturnToPool() 
        {
            OnReturnToPool?.Invoke(this);
        }

        public Action<PooledObject> OnReturnToPool;
        public ISpawnData Data;
    }
}
