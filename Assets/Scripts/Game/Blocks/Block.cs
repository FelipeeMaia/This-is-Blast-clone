using Blast.Data;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Blocks
{
    public class Block : GamePiece, IPoolable<Block>
    {
        [SerializeField] GamePiece _secondCube;

        public int healthPoints { get; private set; }
        public bool isTargetable => (healthPoints > 0 && !_isMoving && !_isTargeted);
        private bool _isTargeted;
        public ISpawnData data { get; set; }

        public Action<Block> ReturnToPool { get; set; }
        public Action<Vector3> OnCubeDestroy;

        [ContextMenu("Destroy Block")]
        public async void TakeDamage()
        {
            healthPoints--;

            if (healthPoints <= 0)
            {
                _renderer.gameObject.SetActive(false);
                await Task.Delay(250);

                //triggers and waits dotween animation
                OnCubeDestroy?.Invoke(transform.position);
                ReturnToPool?.Invoke(this);
            }
            else
            {
                //triggers and waits dotween animation
                OnCubeDestroy?.Invoke(_secondCube.transform.position);
                _secondCube.gameObject.SetActive(false);
            }
        }

        public void Target()
        {
            _isTargeted = true;
        }

        public void OnSpawn(ISpawnData data)
        {
            if (!DataHelper.TryCast(data, out BlockData blockData))
                return;
            
            healthPoints = blockData.healthPoints;
            SetColor(blockData.colorData);
            transform.position = blockData.worldPosition;
            this.data = blockData;
            _isTargeted = false;

            _renderer.gameObject.SetActive(true);

            if (healthPoints == 2)
            {
                _secondCube.gameObject.SetActive(true);
                _secondCube.SetColor(blockData.colorData);
            }
        }
    }
}