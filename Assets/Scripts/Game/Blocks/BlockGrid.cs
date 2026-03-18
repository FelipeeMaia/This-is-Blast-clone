using Blast.Data;
using UnityEngine;

namespace Blast.Game.Blocks
{
    public class BlockGrid : BaseGrid<Block>
    {
        protected override ISpawnData CreateRandomSpawnData(int row, int column)
        {
            var randomColor = GetRandomColor();
            var worldPosition = CalculateGridPosition(row, column);
            var gridPosition = new Vector2(column, row);

            BlockData data = new(randomColor, worldPosition, gridPosition);
            return data;
        }

        public Block GetValidTarget(ColorData shooterColor)
        {
            Block newTarget = null;

            for (int column = 0; column < _columns; column++)
            {
                var block = _grid[0, column];

                if (block.isTargetable/* && block.colorData == shooterColor*/)
                {
                    newTarget = _grid[0, column];
                    break;
                }
            }

            return newTarget;
        }

        async void Start()
        {
            int poolSize = (_rows + 1) * _columns;
            await _pool.CreatePool(_prefab, poolSize, ColapseColumn, _activeParent, _inactiveParent);
            await SpawnGrid();
        }

        
    }
}