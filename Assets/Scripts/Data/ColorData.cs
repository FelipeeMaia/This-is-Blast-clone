using UnityEngine;

namespace Blast.Data
{
    public class ColorData : ScriptableObject, IData
    {
        public Color color;
        public Material material;
        public Colors myColor;
    }

    public enum Colors { Red, Yellow, Orange, Blue, Green, Purple }
}