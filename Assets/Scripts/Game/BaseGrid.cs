using Blast.Data;
using Blast.Interfaces;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blast.Game
{
    /// <summary>
    /// A blueprint for the grids on the game, deals with organization and spawning from object pools.
    /// </summary>
    /// <typeparam name="T">The class that will be organized in a grid.</typeparam>
    public abstract class BaseGrid<T> : MonoBehaviour where T : GamePiece, IPoolable<T>
    {
        [Header("Grid's Stats")]
        [SerializeField] protected int _rows;
        [SerializeField] protected int _columns;
        [SerializeField] protected float _objectSize;
        [SerializeField] protected Vector3 _gridOrigin;
        [SerializeField] protected int _rowDirection = 1;

        [Header("Pool's References")]
        [SerializeField] protected T _prefab;
        [SerializeField] protected ColorData[] _colors;
        [SerializeField] protected Transform _poolParent;

        protected T[,] _grid;
        protected PoolManager _pool;

        protected Action<T> OnSpawn;

        protected async Task SpawnGrid()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    _grid[column, row] = SpawnObject(column, row); ;
                }
            }
        }

        protected abstract ISpawnData CreateRandomSpawnData(int column, int row);

        protected ColorData GetRandomColor()
        {
            int randomIndex = Random.Range(0, _colors.Length);
            var randomColor = _colors[randomIndex];

            return randomColor;
        }

        protected Vector3 CalculateGridPosition(int column, int row)
        {
            Vector3 offsetFromOrigin = new (_objectSize * column, _objectSize * row * _rowDirection);
            var objectPosition = _gridOrigin + offsetFromOrigin;
            return objectPosition;
        }

        protected void ColapseColumn(T destroyedObject)
        {
            var destroyedData = (IGridData)destroyedObject.data;
            int column = (int)destroyedData.gridPosition.x;

            for (int row = 0; row < _rows; row++)
            {
                if (row == _rows - 1)
                {
                    _grid[column, row] = SpawnObject(column, row);
                }
                else
                {
                    _grid[column, row] = _grid[column, row + 1];
                    var blockPosition = CalculateGridPosition(column, row);
                    _grid[column, row].MoveTo(blockPosition);

                    var gridData = (IGridData)_grid[column, row].data;
                    gridData.gridPosition = new(column, row);
                }
            }
        }

        protected T SpawnObject(int column, int row)
        {
            var newObjectData = CreateRandomSpawnData(column, row);
            var newObject = _pool.Spawn<T>(newObjectData);

            OnSpawn?.Invoke(newObject);
            return newObject;
        }

        protected virtual void Awake()
        {
            _pool = FindAnyObjectByType<PoolManager>();
            _grid = new T[_columns, _rows];
        }
    }
}