using Blast.Data;
using Blast.Pooling;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blast.Game.Blocks
{
    public class BlockGrid : MonoBehaviour
    {
        [Header("Grid's Stats")]
        [SerializeField] int _rows;
        [SerializeField] int _columns;
        [SerializeField] float _blockSize;
        [SerializeField] Vector3 _gridOrigin;

        [Header("Grid's References")]
        [SerializeField] Transform _activeParent;
        [SerializeField] Transform _inactiveParent;

        [Header("Block's References")]
        [SerializeField] Block _prefab;
        [SerializeField] ColorData[] _colors;

        private Block[,] _grid;
        private PoolManager _pool;

        private async Task SpawnGrid()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    BlockData data = CreateRandomBlockData(row, column);
                    Block newBlock = _pool.Spawn<Block>(data);
                    _grid[row, column] = newBlock;
                }
            }
        }

        private BlockData CreateRandomBlockData(int row, int column)
        {
            int randomIndex = Random.Range(0, _colors.Length);
            var randomColor = _colors[randomIndex];

            var worldPosition = CalculateBlockPosition(row, column);

            var gridPosition = new Vector2(column, row);

            BlockData data = new(randomColor, worldPosition, gridPosition);
            return data; 
        }

        private Vector3 CalculateBlockPosition(int row, int column)
        {
            var offsetFromOrigin = new Vector3(_blockSize * column, _blockSize * row);
            var blockPosition = _gridOrigin + offsetFromOrigin;
            return blockPosition;
        }

        private void ColapseColumn(Block destroyedBlock)
        {
            var data = (BlockData)destroyedBlock.data;
            int column = (int)data.gridPosition.x;

            for(int row = 0; row < _rows; row++)
            {
                //Debug.Log($"Moving the block on the {row}th row");

                if(row == _rows - 1)
                {
                    var newBlockData = CreateRandomBlockData(row, column);
                    _grid[row, column] = _pool.Spawn<Block>(newBlockData);
                }
                else
                {
                    _grid[row, column] = _grid[row + 1, column];
                    var blockPosition = CalculateBlockPosition(row, column);
                    _grid[row, column].MoveTo(blockPosition);
                }
            }
        }

        public bool GetTarget(out Block target, ColorData shooterColor)
        {
            for(int column = 0; column < _columns; column++)
            {
                var block = _grid[0, column];

                if (block.isTargetable/* && block.colorData == shooterColor*/)
                {
                    target = _grid[0, column];
                    return true;
                }
            }

            target = null;
            return false;
        }

        private void Awake()
        {
            _pool = FindAnyObjectByType<PoolManager>();
            _grid = new Block[_rows, _columns];
        }

        async void Start()
        {
            int poolSize = (_rows + 1) * _columns;
            await _pool.CreatePool<Block>(_prefab, poolSize, ColapseColumn, _activeParent, _inactiveParent);
            await SpawnGrid();
        }

    }
}