using Blast.Game.Blocks;
using UnityEngine;

namespace Blast.Data
{
    public struct BulletData : ISpawnData
    {
        public readonly Block target;
        public readonly Vector3 spawnPosition;

        public BulletData(Block target, Vector3 spawnPosition)
        {
            this.target = target;
            this.spawnPosition = spawnPosition;
        }
    }
}