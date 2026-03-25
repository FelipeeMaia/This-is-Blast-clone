using UnityEngine;

namespace Blast.Interfaces
{
    /// <summary>
    /// Interface to represent objects that can be damaged.
    /// </summary>
    public interface IDamageable
    {
        public void Hit(Vector3 hitPoint);
    }
}