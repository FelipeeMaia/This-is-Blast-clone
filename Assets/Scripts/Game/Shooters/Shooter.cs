using Blast.Data;
using Blast.Game.Blocks;
using Blast.Interfaces;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooters
{
    /// <summary>
    /// Class responsible for the shooter behaviour.
    /// </summary>
    public class Shooter : GamePiece, IClickable
    {
        [Header("Shooting Info")]
        [SerializeField] Transform _shootingPoint;
        [SerializeField] int _timeBetweenShots;
        [SerializeField] float _rotationSpeed;
        [SerializeField] int _ammoLeft;
        [SerializeField] float _recoilForce;

        public event Func<ColorData, Block> OnRequestTarget;
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

        [ContextMenu("Activate")]
        public async void ActivateShooter()
        {
            OnActivate?.Invoke(this);
            Block target = null;

            while (AmmoLeft > 0)
            {
                target = target is null || !target.IsTargetable() ?
                OnRequestTarget?.Invoke(colorData) : target;

                if (target is not null)
                {
                    target.Target();
                    await LookTo(target.transform);
                    await Task.Delay(_timeBetweenShots);

                    CallShot(target);
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
            //transform.rotation = Quaternion.Euler(0, 0, targetAngle);

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

            transform.DOShakePosition(0.1f, _recoilForce);
            BulletData bulletData = new(target, target.transform.position, _shootingPoint.position);
            OnShoot?.Invoke(bulletData);
        }

        public override void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out ShooterData shooterData))
                return;

            Data = shooterData;

            SetColor(shooterData.colorData);
            AmmoLeft = shooterData.ammountOfBullets;
            transform.position = shooterData.spawnPosition;
        }

        public override void ReturnToPool()
        {
            OnShoot = null;
            OnActivate = null;
            OnRequestTarget = null;
            base.ReturnToPool();
        }

        private void Start()
        {
            //_blockGrid = FindAnyObjectByType<BlockGrid>();
        }
    }
}