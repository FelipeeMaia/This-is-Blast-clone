using Blast.Data;
using Blast.Interfaces;
using UnityEngine;

namespace Blast.Game.Shooters
{
    /// <summary>
    /// Class reponsible for managing the Grid of Shooters in the game.
    /// </summary>
    public class ShooterGrid : BaseGrid<Shooter>
    {
        [Header("Bullet Pool")]
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] int _bulletCap;
        [SerializeField] Transform _bulletPoolParent;

        protected override ISpawnData CreateRandomSpawnData(int column, int row)
        {
            var randomColor = GetRandomColor();
            var worldPosition = CalculateGridPosition(column, row);
            var gridPosition = new Vector2(column, row);

            ShooterData data = new(randomColor, worldPosition, gridPosition);
            return data;
        }

        private void ListenToShooter(Shooter shooter)
        {
            shooter.OnShoot += bulletData => _pool.Spawn<Bullet>(bulletData);
            shooter.OnActivate += ColapseColumn;
        }

        async void Start()
        {
            OnSpawn += ListenToShooter;

            int poolSize = (_rows + 1) * _columns;
            await _pool.CreatePool(_prefab, poolSize, _poolParent);

            await _pool.CreatePool(_bulletPrefab, _bulletCap, _bulletPoolParent);

            await SpawnGrid();
        }
    }
}