using System;

namespace Blast.Interfaces
{
    /// <summary>
    /// Interface for objects that can be put in an ObjectPool.
    /// </summary>
    public interface IPoolable
    {
        public void OnSpawn(ISpawnData spawnData);
        public void ReturnToPool();

        public Action<IPoolable> OnReturnToPool { get; set; }

        public ISpawnData data { get; set; }
    }
}