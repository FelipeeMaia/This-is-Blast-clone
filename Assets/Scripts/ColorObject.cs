using UnityEngine;

namespace Blast
{
    public abstract class ColorObject : MonoBehaviour
    {
        public ColorInfo colorInfo { get; private set; }
        [SerializeField] protected Renderer _renderer;

        public void SetColor(ColorInfo newColorInfo)
        {
            colorInfo = newColorInfo;
            _renderer.material = colorInfo.material;
        }
    }
}