using UnityEngine;

namespace Blast
{
    public class ColorInfo : ScriptableObject
    {
        public Color color;
        public Material material;
        public Colors colorName;
    }

    public enum Colors { Red, Yellow, Orange, Blue, Green, Purple }
}