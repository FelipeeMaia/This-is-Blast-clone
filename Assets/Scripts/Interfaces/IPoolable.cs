using System;

namespace Blast.Interfaces
{
    /// <summary>
    /// Interface for objects that can be put in an ObjectPool.
    /// </summary>
    /// <typeparam name="T">The class of the object.</typeparam>
    public interface IPoolable<T>
    {
        public void OnSpawn(ISpawnData spawnData);
        public void ReturnToPool();

        public Action<T> OnReturnToPool { get; set; }

        public ISpawnData data { get; set; }
    }
}