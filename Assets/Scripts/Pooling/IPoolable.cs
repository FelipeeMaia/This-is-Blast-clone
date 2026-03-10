using System;

namespace Blast.Pooling
{
    public interface IPoolable<TObject, TInfo>
    {
        public void OnSpawn(TInfo spawnInfo);

        public Action<TObject> ReturnToPool { get; set; }
    }

    public interface IPoolable
    {
        public void OnSpawn();

        public void OnDespawn();
    }
}