using Blast.Interfaces;
using UnityEngine;

namespace Blast.Data
{
    public struct BlockData : ISpawnData, IGridData
    {
        public readonly ColorData colorData;
        public readonly int healthPoints;

        public Vector2 gridPosition { get; set; }
        public readonly Vector3 worldPosition;

        public BlockData(ColorData colorData, Vector3 worldPosition, Vector2 gridPosition, int healthPoints = 1)
        {
            this.colorData = colorData;
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
            this.healthPoints = healthPoints;
        }
    }
}