using Blast.Data;
using System;

namespace Blast.Pooling
{
    public interface IPoolable<T>
    {
        public void OnSpawn(IData spawnData);

        public Action<T> ReturnToPool { get; set; }

        public IData data { get; set; }
    }
}