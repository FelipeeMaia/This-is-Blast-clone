using Blast.Data;
using Blast.Interfaces;
using Blast.Core;
using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Blast.Game.Blocks
{
    /// <summary>
    /// Class responsible for the blocks behaviour.
    /// </summary>
    public class Block : GamePiece, IHitable
    {
        [SerializeField] GamePiece _secondCube;

        public int healthPoints { get; private set; }
        private bool _isTargeted;

        public Action<Vector3> OnCubeDestroy;

        [Header("Punch Effect")]
        [SerializeField] float _punchStrength;
        [SerializeField] float _punchDuration;

        [Header("Shrink Effect")]
        [SerializeField] float _shrinkDuration;

        public async void Hit(Vector3 hitPoint)
        {
            healthPoints--;

            if (healthPoints <= 0)
            {
                await HitBlock(transform, hitPoint);
                ReturnToPool();
            }
            else
            {
                await HitBlock(_secondCube.transform, hitPoint);
                _secondCube.gameObject.SetActive(false);
            }
        }

        private async Task HitBlock(Transform target, Vector3 hitPoint)
        {
            await target.DoPunchEffect(hitPoint, _punchStrength, _punchDuration);
            await target.DoShrinkOut(_shrinkDuration);
            OnCubeDestroy?.Invoke(target.position);
        }

        public void Target()
        {
            _isTargeted = true;
        }

        public bool IsTargetable() => (healthPoints > 0 && !_isMoving && !_isTargeted);

        public override void OnSpawn(ISpawnData data)
        {
            if (!DataHelper.TryCast(data, out BlockData blockData))
                return;
            
            healthPoints = blockData.healthPoints;
            SetColor(blockData.colorData);
            transform.position = blockData.worldPosition;
            this.Data = blockData;
            _isTargeted = false;

            transform.localScale = Vector3.one;
            _renderer.gameObject.SetActive(true);

            if (healthPoints > 1)
            {
                _secondCube.transform.localScale = Vector3.one;
                _secondCube.gameObject.SetActive(true);
                _secondCube.SetColor(blockData.colorData);
            }
        }
    }
}