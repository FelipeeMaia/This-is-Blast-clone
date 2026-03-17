using Blast.Data;
using Blast.Game.Blocks;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooter
{
    public class Bullet : GamePiece, IPoolable<Bullet>
    {
        public Action<Bullet> ReturnToPool { get; set; }
        public ISpawnData data { get; set; }

        private Block _myTarget;

        public async void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out BulletData bulletData))
                return;

            data = bulletData;
            _myTarget = bulletData.target;
            transform.position = bulletData.spawnPosition;

            await MoveTo(_myTarget.transform.position);

            _myTarget.TakeDamage();
            ReturnToPool?.Invoke(this);
        }

        public override async Task MoveTo(Vector3 targetPosition)
        {
            await base.MoveTo(targetPosition);
        }
    }
}