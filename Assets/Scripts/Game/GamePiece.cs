using Blast.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Game
{
    /// <summary>
    /// Blueprint for the movable pieces in the game.
    /// </summary>
    public abstract class GamePiece : MonoBehaviour
    {
        public ColorData colorData { get; private set; }
        [SerializeField] protected Renderer _renderer;

        protected bool _isMoving; 
        [SerializeField] float _moveSpeed;
        private const int FIXED_TIME = 20;

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
                    (newPosition, targetPosition, _moveSpeed * Time.fixedDeltaTime);

                transform.position = newPosition;
                await Task.Delay(FIXED_TIME);
            }

            _isMoving = false;
        }
    }
}