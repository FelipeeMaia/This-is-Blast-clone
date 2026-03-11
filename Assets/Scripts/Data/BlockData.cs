using UnityEngine;

namespace Blast.Data
{
    public struct BlockData : ISpawnData
    {
        public readonly ColorData colorData;
        public readonly Vector3 position;
        public readonly int healthPoints;

        public BlockData(ColorData colorInfo, Vector3 position, int health = 1)
        {
            this.colorData = colorInfo;
            this.position = position;
            this.healthPoints = health;
        }
    }
}