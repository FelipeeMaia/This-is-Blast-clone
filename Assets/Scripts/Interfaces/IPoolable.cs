using System;

namespace Blast.Interfaces
{
    public interface IPoolable<T>
    {
        public void OnSpawn(ISpawnData spawnData);
        public void ReturnToPool();

        public Action<T> OnReturnToPool { get; set; }

        public ISpawnData data { get; set; }
    }
}