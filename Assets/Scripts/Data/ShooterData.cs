using UnityEngine;

namespace Blast.Data
{
    public struct ShooterData : ISpawnData
    { 
        public readonly Vector3 spawnPosition;
        public readonly ColorData colorData;
        public readonly int ammountOfBullets;
    }
}