using Blast.Data;
using Blast.Interfaces;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Shooters
{
    /// <summary>
    /// Class responsible for the Bullet behaviour.
    /// </summary>
    public class Bullet : GamePiece
    {
        private IHitable _myTarget;
        private Vector3 _targetPosition;
        [SerializeField] float _distanceToHit;

        public override async void OnSpawn(ISpawnData spawnData)
        {
            if (!DataHelper.TryCast(spawnData, out BulletData bulletData))
                return;

            Data = bulletData;

            _myTarget = bulletData.target;
            _targetPosition = bulletData.targetPosition;
            transform.position = bulletData.spawnPosition;

            await MoveTo(_targetPosition);

            _myTarget.Hit(transform.position);
            ReturnToPool();
        }

        public override async Task MoveTo(Vector3 targetPosition, float distanceToGoal = 0.01f)
        {
            await base.MoveTo(targetPosition, _distanceToHit);
        }
    }
}