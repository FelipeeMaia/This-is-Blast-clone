using Blast.Interfaces;
using UnityEngine;

namespace Blast.Data
{
    /// <summary>
    /// Struct that transports the SpawnData needed by Bullets.
    /// </summary>
    public struct BulletData : ISpawnData
    {
        public readonly IHitable target;
        public readonly Vector3 targetPosition;
        public readonly Vector3 spawnPosition;

        public BulletData(IHitable target, Vector3 targetPosition, Vector3 spawnPosition)
        {
            this.target = target;
            this.targetPosition = targetPosition;
            this.spawnPosition = spawnPosition;
        }
    }
}