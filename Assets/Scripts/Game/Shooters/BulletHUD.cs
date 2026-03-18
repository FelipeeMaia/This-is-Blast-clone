using TMPro;
using UnityEngine;

namespace Blast.Game.Shooter.HUD
{
    public class BulletHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Shooter _myShooter;
        [SerializeField] TMP_Text _counter;

        private void Start()
        {
            _myShooter.OnAmmoChange += UpdateCounter;
        }

        private void UpdateCounter(int ammoLeft)
        {
            _counter.text = $"{ammoLeft}";
        }
    }
}