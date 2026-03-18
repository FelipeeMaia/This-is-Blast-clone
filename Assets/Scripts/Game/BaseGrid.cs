using Blast.Data;
using Blast.Pooling;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game
{
    public abstract class BaseGrid<T> : MonoBehaviour where T : GamePiece, IPoolable<T>
    {
        [Header("Grid's Stats")]
        [SerializeField] protected int _rows;
        [SerializeField] protected int _columns;
        [SerializeField] protected float _objectSize;
        [SerializeField] protected Vector3 _gridOrigin;
        [SerializeField] protected int _rowDirection = 1;

        [Header("Grid's References")]
        [SerializeField] protected Transform _activeParent;
        [SerializeField] protected Transform _inactiveParent;

        [Header("Block's References")]
        [SerializeField] protected T _prefab;
        [SerializeField] protected ColorData[] _colors;

        protected T[,] _grid;
        protected PoolManager _pool;

        protected async Task SpawnGrid()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    ISpawnData data = CreateRandomSpawnData(row, column);
                    T newObject = _pool.Spawn<T>(data);
                    _grid[row, column] = newObject;
                }
            }
        }

        protected abstract ISpawnData CreateRandomSpawnData(int row, int column);

        protected ColorData GetRandomColor()
        {
            int randomIndex = Random.Range(0, _colors.Length);
            var randomColor = _colors[randomIndex];

            return randomColor;
        }

        protected Vector3 CalculateGridPosition(int row, int column)
        {
            Vector3 offsetFromOrigin = new (_objectSize * column, _objectSize * row * _rowDirection);
            var objectPosition = _gridOrigin + offsetFromOrigin;
            return objectPosition;
        }

        protected void ColapseColumn(T destroyedObject)
        {
            var data = (IGridData)destroyedObject.data;
            int column = (int)data.gridPosition.x;

            for (int row = 0; row < _rows; row++)
            {
                if (row == _rows - 1)
                {
                    var newObjectData = CreateRandomSpawnData(row, column);
                    _grid[row, column] = _pool.Spawn<T>(newObjectData);
                }
                else
                {
                    _grid[row, column] = _grid[row + 1, column];
                    var blockPosition = CalculateGridPosition(row, column);
                    _grid[row, column].MoveTo(blockPosition);
                }
            }
        }

        protected virtual void Awake()
        {
            _pool = FindAnyObjectByType<PoolManager>();
            _grid = new T[_rows, _columns];
        }
    }
}