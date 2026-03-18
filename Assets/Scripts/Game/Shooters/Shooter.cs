using Blast.Data;
using Blast.Game.Blocks;
using Blast.Interfaces;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Blast.Game.Shooter
{
    public class Shooter : GamePiece, IPoolable<Shooter>, IPointerClickHandler
    {
        [SerializeField] int _ammoLeft;
        [SerializeField] int _timeBetweenShots;
        [SerializeField] float _rotationSpeed;
        [SerializeField] PoolManager _pool;
        private BlockGrid _blockGrid;
        public ISpawnData data { get; set; }
        public Action<Shooter> OnReturnToPool { get; set; }

        [ContextMenu("Activate")]
        public async void ActivateShooter()
        {
            Block target = null;

            while (_ammoLeft > 0)
            {
                target = target is null || !target.isTargetable ?
                _blockGrid.GetValidTarget(colorData) : target;

                if (target is not null)
                {
                    target.Target();
                    await LookTo(target.transform);
                    ShootTarget(target);

                    await Task.Delay(_timeBetweenShots);
                }
                else
                {
                    await Task.Yield();
                }
            }

            ReturnToPool();
        }

        public async Task LookTo(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            direction.z = 0f;

            if (direction == Vector3.zero) return;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float angleDifference = 999f;
            while (angleDifference > 0.5f)
            {
                float currentAngle = transform.eulerAngles.z;

                float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                _rotationSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0, 0, newAngle);
                angleDifference = Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle));

                await Task.Yield();
            }
        }

        public void ShootTarget(Block target)
        {
            BulletData data = new(target, target.transform.position, transform.position);
            var bullet = _pool.Spawn<Bullet>(data);

            _ammoLeft--;
        }

        public void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out ShooterData shooterData))
                return;

            data = shooterData;

            SetColor(shooterData.colorData);
            _ammoLeft = shooterData.ammountOfBullets;
            transform.position = shooterData.spawnPosition;
        }

        public void ReturnToPool() => OnReturnToPool?.Invoke(this);

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clickei no {gameObject.name}!");
        }

        private void Start()
        {
            _blockGrid = FindAnyObjectByType<BlockGrid>();
        }
    }
}