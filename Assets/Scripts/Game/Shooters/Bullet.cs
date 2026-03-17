using Blast.Data;
using Blast.Pooling;
using System;
using UnityEngine;

namespace Blast.Game.Shooter
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
        public Action<Bullet> ReturnToPool { get; set; }
        public ISpawnData data { get; set; }

        public void OnSpawn(ISpawnData spawnData)
        {
            throw new NotImplementedException();
        }
    }
}