using Blast.Data;
using UnityEngine;

namespace Blast
{
    public abstract class ColorObject : MonoBehaviour
    {
        public ColorData colorData { get; private set; }
        [SerializeField] protected Renderer _renderer;

        public void SetColor(ColorData newColorData)
        {
            colorData = newColorData;
            _renderer.material = colorData.material;
        }
    }
}