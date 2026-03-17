using Blast.Game.Blocks;
using Blast.Game.Shooter;
using Blast.Pooling;
using UnityEngine;

public class ShooterGrid : MonoBehaviour
{
    private PoolManager _pool;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] int _bulletCap;

    //[SerializeField] Shooter _shooterPrefab;

    private void Awake()
    {
        _pool = FindAnyObjectByType<PoolManager>();
    }

    async void Start()
    {
        await _pool.CreatePool(_bulletPrefab, _bulletCap, null, transform, transform);
    }
}
