using Blast.Data;
using Blast.Interfaces;
using Blast.Pooling;
using UnityEngine;

namespace Blast.Game.Shooters
{
    /// <summary>
    /// Class reponsible for managing the Grid of Shooters in the game.
    /// </summary>
    public class ShooterGrid : BaseGrid<Shooter>
    {
        protected override ISpawnData CreateRandomSpawnData(int column, int row)
        {
            var randomColor = GetRandomColor();
            var worldPosition = CalculateGridPosition(column, row);
            var gridPosition = new Vector2(column, row);

            ShooterData data = new(randomColor, worldPosition, gridPosition);
            return data;
        }

        private void SetShooterListeners(PooledObject pooledShooter)
        {
            if (pooledShooter is not Shooter shooter) return;

            shooter.OnShoot += bulletData => _pools.Spawn<Bullet>(bulletData);
            shooter.OnActivate += ColapseColumn;
        }

        async void Start()
        {
            OnSpawn += SetShooterListeners;

            int poolSize = (_rows + 1) * _columns;
            await _pools.CreatePool(_prefab, poolSize, _poolParent);

            await SpawnGrid();
        }
    }
}