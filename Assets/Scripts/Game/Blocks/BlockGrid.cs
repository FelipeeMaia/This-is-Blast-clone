using Blast.Data;
using Blast.Interfaces;
using UnityEngine;

namespace Blast.Game.Blocks
{
    public class BlockGrid : BaseGrid<Block>
    {
        protected override ISpawnData CreateRandomSpawnData(int column, int row)
        {
            var randomColor = GetRandomColor();
            var worldPosition = CalculateGridPosition(column, row);
            var gridPosition = new Vector2(column, row);

            BlockData data = new(randomColor, worldPosition, gridPosition);
            return data;
        }

        public Block GetValidTarget(ColorData shooterColor)
        {
            Block newTarget = null;

            for (int column = 0; column < _columns; column++)
            {
                var block = _grid[column, 0];

                if (block.isTargetable/* && block.colorData == shooterColor*/)
                {
                    newTarget = _grid[column, 0];
                    break;
                }
            }

            return newTarget;
        }

        async void Start()
        {
            int poolSize = _columns * (_rows + 1);
            await _pool.CreatePool(_prefab, poolSize, _poolParent, ColapseColumn);
            await SpawnGrid();
        }

        
    }
}