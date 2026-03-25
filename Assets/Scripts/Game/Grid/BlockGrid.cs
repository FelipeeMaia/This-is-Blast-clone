using Blast.Data;
using Blast.Game.Shooters;
using Blast.Interfaces;
using Blast.Pooling;
using System;
using UnityEngine;

namespace Blast.Game.Blocks
{
    /// <summary>
    /// Class reponsible for managing the Grid of Blocks in the game.
    /// </summary>
    public class BlockGrid : BaseGrid<Block>
    {
        [SerializeField] ShooterGrid _shooterGrid;

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

                if (block.IsTargetable()/* && block.colorData == shooterColor*/)
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
            await _pools.CreatePool(_prefab, poolSize, _poolParent, ColapseColumn);
            await SpawnGrid();
        }

        protected override void Awake()
        {
            base.Awake();

            if (_shooterGrid is null)
                _shooterGrid = FindAnyObjectByType<ShooterGrid>();

            _shooterGrid.OnSpawn += ListenToShooterRequest;
        }

        private void ListenToShooterRequest(PooledObject pooledShooter)
        {
            if (pooledShooter is not Shooter shooter) return;
            shooter.OnRequestTarget += GetValidTarget;
        }
    }
}