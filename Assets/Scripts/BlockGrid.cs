using Blast.Data;
using Blast.Pooling;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blast
{
    public class BlockGrid : MonoBehaviour
    {
        [Header("Grid's Stats")]
        [SerializeField] int _rows;
        [SerializeField] int _columns;
        [SerializeField] float _blockSize;
        [SerializeField] Vector3 _gridOrigin;

        [Header("Block's References")]
        [SerializeField] Block _prefab;
        [SerializeField] ColorData[] _colors;

        private Block[,] _grid;
        private PoolManager _pool;

        private async Task SpawnGrid()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    int randomIndex = Random.Range(0, _colors.Length - 1);
                    var randomColor = _colors[randomIndex];

                    var offsetFromOrigin = new Vector3(_blockSize * i, _blockSize * j);
                    var blockPosition = _gridOrigin + offsetFromOrigin;

                    BlockData data = new BlockData(randomColor, blockPosition);
                    Block newBlock = _pool.Spawn<Block>(data);

                    _grid[i, j] = newBlock;
                }
            }
        }

        private void Awake()
        {
            _pool = FindAnyObjectByType<PoolManager>();
            _grid = new Block[_rows, _columns];
        }

        async void Start()
        {
            int gridSize = _rows * _columns;
            await _pool.CreatePool<Block>(_prefab, gridSize, transform);
            await SpawnGrid();
        }

    }
}