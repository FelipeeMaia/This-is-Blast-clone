using Blast.Data;
using Blast.Interfaces;
using Blast.Game.Blocks;

using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooters
{
    public class Shooter : GamePiece, IPoolable<Shooter>, IClickable
    {
        [SerializeField] int _timeBetweenShots;
        [SerializeField] float _rotationSpeed;
        [SerializeField] int _ammoLeft;

        public ISpawnData data { get; set; }
        public Action<Shooter> OnReturnToPool { get; set; }
        public Action<Shooter> OnActivate { get; set; }
        public Action<ISpawnData> OnShoot { get; set; }
        public Action<int> OnAmmoChange { get; set; }
        private int AmmoLeft
        {
            get { return _ammoLeft; }
            set
            {
                _ammoLeft = value;
                OnAmmoChange?.Invoke(_ammoLeft);
            }

        }

        private BlockGrid _blockGrid;
        [ContextMenu("Activate")]
        public async void ActivateShooter()
        {
            OnActivate?.Invoke(this);
            Block target = null;

            while (AmmoLeft > 0)
            {
                target = target is null || !target.isTargetable ?
                _blockGrid.GetValidTarget(colorData) : target;

                if (target is not null)
                {
                    target.Target();
                    await LookTo(target.transform);
                    CallShot(target);

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

        public void CallShot(Block target)
        {
            AmmoLeft--;

            BulletData bulletData = new(target, target.transform.position, transform.position);
            OnShoot?.Invoke(bulletData);
        }

        public void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out ShooterData shooterData))
                return;

            data = shooterData;

            SetColor(shooterData.colorData);
            AmmoLeft = shooterData.ammountOfBullets;
            transform.position = shooterData.spawnPosition;
        }

        public void ReturnToPool()
        {
            OnShoot = null;
            OnActivate = null;
            OnReturnToPool?.Invoke(this);
        }

        private void Start()
        {
            _blockGrid = FindAnyObjectByType<BlockGrid>();
        }
    }
}