using UnityEngine;

namespace Blast.Data
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Data/ColorData", order = 1)]
    public class ColorData : ScriptableObject
    {
        public Color color;
        public Material material;
        public Colors myColor;
    }

    public enum Colors { Red, Yellow, Orange, Blue, Green, Purple }
}