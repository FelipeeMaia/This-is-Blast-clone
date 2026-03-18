using Blast.Interfaces;
using UnityEngine;

namespace Blast.Data
{
    public struct BulletData : ISpawnData
    {
        public readonly IDamageble target;
        public readonly Vector3 targetPosition;
        public readonly Vector3 spawnPosition;

        public BulletData(IDamageble target, Vector3 targetPosition, Vector3 spawnPosition)
        {
            this.target = target;
            this.targetPosition = targetPosition;
            this.spawnPosition = spawnPosition;
        }
    }
}