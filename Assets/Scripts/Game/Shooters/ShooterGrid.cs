using Blast.Data;
using Blast.Interfaces;
using UnityEngine;

namespace Blast.Game.Shooter
{
    public class ShooterGrid : BaseGrid<Shooter>
    {
        [Header("Bullet Pool")]
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] int _bulletCap;
        [SerializeField] Transform _bulletPoolParent;

        protected override ISpawnData CreateRandomSpawnData(int row, int column)
        {
            var randomColor = GetRandomColor();
            var worldPosition = CalculateGridPosition(row, column);
            var gridPosition = new Vector2(column, row);

            ShooterData data = new(randomColor, worldPosition, gridPosition);
            return data;
        }

        async void Start()
        {
            int poolSize = (_rows + 1) * _columns;
            await _pool.CreatePool(_prefab, poolSize, _poolParent, ColapseColumn);

            await _pool.CreatePool(_bulletPrefab, _bulletCap, _bulletPoolParent);

            await SpawnGrid();
        }
    }
}