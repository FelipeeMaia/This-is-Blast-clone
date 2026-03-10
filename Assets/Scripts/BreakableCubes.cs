using Blast.Pooling;
using System;
using UnityEngine;

namespace Blast
{
    public class BreakableCube : ColorObject, IPoolable<BreakableCube, BreakableCubeInfo>
    {
        [SerializeField] ColorObject _secondCube;
        public int healthPoints { get; private set; }
        public bool isDying => (healthPoints <= 0);

        public Action<BreakableCube> ReturnToPool { get; set; }

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

        public void OnSpawn(BreakableCubeInfo spawnInfo)
        {
            healthPoints = spawnInfo.healthPoints;
            SetColor(spawnInfo.colorInfo);

            if (healthPoints == 2)
            {
                _secondCube.gameObject.SetActive(true);
                _secondCube.SetColor(spawnInfo.colorInfo);
            }
        }
    }
}