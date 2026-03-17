using Blast.Data;
using Blast.Game.Blocks;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooter
{
    public class Shooter : ColorObject, IPoolable<Shooter>
    {
        [SerializeField] int _ammoLeft;
        [SerializeField] int _timeBetweenShots;
        [SerializeField] float _rotationSpeed;
        [SerializeField] float _moveSpeed;
        private BlockGrid _blockGrid;

        public ISpawnData data { get; set; }
        public Action<Shooter> ReturnToPool { get; set; }

        public void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast<ShooterData>(data, out ShooterData shooterData))
                return;

            data = shooterData;

            SetColor(shooterData.colorData);
            _ammoLeft = shooterData.ammountOfBullets;
            transform.position = shooterData.spawnPosition;
        }

        private void Start()
        {
            _blockGrid = FindAnyObjectByType<BlockGrid>();
        }

        [ContextMenu("Activate")]
        public async void ActivateShooter()
        {
            while (_ammoLeft > 0)
            {
                if(_blockGrid.GetTarget(out Block newTarget, colorData))
                {
                    await LookTo(newTarget.transform);
                    ShootTarget(newTarget);

                    await Task.Delay(_timeBetweenShots);
                }
                else
                {
                    await Task.Yield();
                }
            }

            //DestroyShooter
            gameObject.SetActive(false);
            //ReturnToPool?.Invoke(this);
        }

        public async Task LookTo(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            direction.z = 0f;

            if (direction == Vector3.zero) return;

            //var targetRotation = Quaternion.LookRotation(direction);
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float angleDifference = 999f;
            while (angleDifference > 0.5f)
            {
                /*transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );*/

                //angle = Quaternion.Angle(transform.rotation, targetRotation);
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
            _ammoLeft--;

            target.TakeDamage();
        }

        public void MoveTo(Vector3 position)
        {

        }
    }
}