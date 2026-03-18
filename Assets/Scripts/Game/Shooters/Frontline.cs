using System;
using UnityEngine;

namespace Blast.Game.Shooter
{
    public class Frontline : MonoBehaviour
    {
        [SerializeField] Transform[] _shooterSlots;
        Shooter[] _frontShooters;

        int _slotsCount;

        public void SetSlots(int slotsCount)
        {
            if (slotsCount < 2 || slotsCount > 5)
            {
                Debug.LogError($"{slotsCount} is an invalid slots ammount, must be between 2 and 5.");
                return;
            }

            _slotsCount = slotsCount;
            CleanShooters();
            OrganizeSlots();
        }

        private void CleanShooters()
        {
            if (_frontShooters is null) return;

            for(int i = 0; i<  _frontShooters.Length; i++)
            {
                var shooter = _frontShooters[i];
                if (shooter is not null) shooter.ReturnToPool();
            }

            _frontShooters = new Shooter[_slotsCount];
        }

        private void OrganizeSlots()
        {
            throw new NotImplementedException();
        }

        public async void SelectShooter(Shooter selectedShooter)
        {
            int slotFound = -1;
            for (int i = 0; i < _slotsCount; i++)
            {
                if (_frontShooters[i] is null)
                {
                    _frontShooters[i] = selectedShooter;
                    slotFound = i;
                    break;
                }
            }

            if (slotFound == -1) return;

            selectedShooter.OnReturnToPool += RemoveShooter;

            var slotPosition = _shooterSlots[slotFound].position;
            await selectedShooter.MoveTo(slotPosition);
            selectedShooter.ActivateShooter();
        }

        private void RemoveShooter(Shooter shooterToRemove)
        {
            for (int i = 0; i < _slotsCount; i++)
            {
                if (_frontShooters[i] == shooterToRemove)
                {
                    _frontShooters[i] = null;
                    break;
                }
            }
        }
    }
}