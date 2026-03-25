using UnityEngine;

namespace Blast.Interfaces
{
    /// <summary>
    /// Interface to represent objects that can be damaged.
    /// </summary>
    public interface IHitable
    {
        public void Hit(Vector3 hitPoint);

        public void Target();

        public bool IsTargetable();
    }
}