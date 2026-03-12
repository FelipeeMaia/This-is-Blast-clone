using Blast.Data;
using Blast.Pooling;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game.Blocks
{
    public class Block : ColorObject, IPoolable<Block>
    {
        [SerializeField] ColorObject _secondCube;
        [SerializeField] float _fallSpeed;

        public int healthPoints { get; private set; }
        public bool isTargetable => (healthPoints > 0 && !_isMoving);
        public ISpawnData data { get; set; }

        private bool _isMoving;

        public Action<Block> ReturnToPool { get; set; }
        public Action<Vector3> OnCubeDestroy;

        [ContextMenu("Damage")]
        public void TakeDamage()
        {
            healthPoints--;

            if (healthPoints <= 0)
            {
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

        public async Task MoveTo(Vector3 targetPosition)
        {
            Vector3 newPosition = transform.position;
            _isMoving = true;

            while (Vector3.Distance(newPosition, targetPosition) > 0.01f)
            {
                newPosition = Vector3.MoveTowards(newPosition, targetPosition, 
                                                  _fallSpeed * Time.deltaTime);

                transform.position = newPosition;
                await Task.Yield();
            }

            _isMoving = false;
        }

        public void OnSpawn(ISpawnData data)
        {
            if (!DataHelper.TryCast<BlockData>(data, out BlockData blockData))
                return;
            
            healthPoints = blockData.healthPoints;
            SetColor(blockData.colorData);
            transform.position = blockData.worldPosition;
            this.data = blockData;

            if (healthPoints == 2)
            {
                _secondCube.gameObject.SetActive(true);
                _secondCube.SetColor(blockData.colorData);
            }
        }
    }
}