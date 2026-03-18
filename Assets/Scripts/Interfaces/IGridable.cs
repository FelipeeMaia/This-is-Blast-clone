using UnityEngine;

namespace Blast.Interfaces
{
    public interface IGridData : ISpawnData
    {
        public Vector2 gridPosition { get; set; }
    }
}