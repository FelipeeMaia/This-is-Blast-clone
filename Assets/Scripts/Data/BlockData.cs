using UnityEngine;

namespace Blast.Data
{
    public struct BlockData : ISpawnData
    {
        public readonly ColorData colorData;
        public readonly Vector3 worldPosition;
        public readonly Vector2 gridPosition;
        public readonly int healthPoints;

        public BlockData(ColorData colorData, Vector3 worldPosition, Vector2 gridPosition, int healthPoints = 1)
        {
            this.colorData = colorData;
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
            this.healthPoints = healthPoints;
        }
    }
}