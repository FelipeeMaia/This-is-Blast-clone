using Blast.Data;
using System;

namespace Blast.Pooling
{
    public interface IPoolable<T>
    {
        public void OnSpawn(ISpawnData spawnData);

        public Action<T> ReturnToPool { get; set; }

        public ISpawnData data { get; set; }
    }
}