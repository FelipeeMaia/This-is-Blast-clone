using UnityEngine;

namespace Blast.Interfaces
{
    /// <summary>
    /// Interface that represents SpawnData for objects in a grid.
    /// </summary>
    public interface IGridData : ISpawnData
    {
        public Vector2 gridPosition { get; set; }
    }
}