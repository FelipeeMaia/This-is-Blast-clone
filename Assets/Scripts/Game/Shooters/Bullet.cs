using Blast.Data;
using Blast.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooters
{
    public class Bullet : GamePiece, IPoolable<Bullet>
    {
        public Action<Bullet> OnReturnToPool { get; set; }
        public ISpawnData data { get; set; }

        private IDamageble _myTarget;
        private Vector3 _targetPosition;

        public async void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out BulletData bulletData))
                return;

            data = bulletData;

            _myTarget = bulletData.target;
            _targetPosition = bulletData.targetPosition;
            transform.position = bulletData.spawnPosition;

            await MoveTo(_targetPosition);

            _myTarget.Damage();
            ReturnToPool();
        }

        public void ReturnToPool() => OnReturnToPool?.Invoke(this);

        public override async Task MoveTo(Vector3 targetPosition)
        {
            await base.MoveTo(targetPosition);
        }
    }
}