using Blast.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game
{
    public abstract class GamePiece : MonoBehaviour
    {
        public ColorData colorData { get; private set; }
        [SerializeField] protected Renderer _renderer;

        protected bool _isMoving; 
        [SerializeField] float _moveSpeed;

        public virtual void SetColor(ColorData newColorData)
        {
            colorData = newColorData;
            _renderer.material = colorData.material;
        }

        public virtual async Task MoveTo(Vector3 targetPosition)
        {
            Vector3 newPosition = transform.position;
            _isMoving = true;

            while (Vector3.Distance(newPosition, targetPosition) > 0.01f)
            {
                newPosition = Vector3.MoveTowards
                    (newPosition, targetPosition, _moveSpeed * Time.deltaTime);

                transform.position = newPosition;
                await Task.Yield();
            }

            _isMoving = false;
        }
    }
}