using Blast.Interfaces;
using UnityEngine;

namespace Blast.Data
{
    /// <summary>
    /// Struct that transports the SpawnData needed by Shooters.
    /// </summary>
    public struct ShooterData : IGridData
    { 
        public readonly ColorData colorData;
        public readonly int ammountOfBullets;

        public Vector2 gridPosition { get; set; }
        public readonly Vector3 spawnPosition;

        public ShooterData(ColorData colorData, Vector3 spawnPosition, Vector2 gridPosition, int ammountOfBullets = 20)
        {
            this.colorData = colorData;
            this.spawnPosition = spawnPosition;
            this.gridPosition = gridPosition;
            this.ammountOfBullets = ammountOfBullets;
        }

    }
}