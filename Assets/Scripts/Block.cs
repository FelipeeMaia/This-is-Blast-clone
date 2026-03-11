using Blast.Data;
using Blast.Pooling;
using System;
using UnityEngine;

namespace Blast
{
    public class Block : ColorObject, IPoolable<Block>
    {
        [SerializeField] ColorObject _secondCube;
        public int healthPoints { get; private set; }
        public bool isDying => (healthPoints <= 0);

        public Action<Block> ReturnToPool { get; set; }
        public ISpawnData data { get; set; }

        public Action<Vector3> OnCubeDestroy;

        public void TakeDamage()
        {
            healthPoints--;

            if (healthPoints < 0)
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

        public void OnSpawn(ISpawnData data)
        {
            if (!DataHelper.TryCast<BlockData>(data, out BlockData blockData))
                return;
            
            healthPoints = blockData.healthPoints;
            SetColor(blockData.colorData);
            transform.position = blockData.position;

            if (healthPoints == 2)
            {
                _secondCube.gameObject.SetActive(true);
                _secondCube.SetColor(blockData.colorData);
            }
        }
    }
}