using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace Blast.Core
{
    public static class TweenHelper
    {
        public static async Task PunchEffect(this Transform target, Vector3 impactPoint, float strength, float duration)
        {
            Vector3 hitDirection = impactPoint - target.position;
            hitDirection.z = 0f;    
            Vector3 punch = -hitDirection.normalized * strength;

            var punchSequence = DOTween.Sequence();
            punchSequence.Append(target.DOPunchPosition(punch, duration, 0));

            var popSequence = DOTween.Sequence();
            popSequence.Append(target.DOScale(1.05f, duration / 3).SetEase(Ease.OutQuad));
            popSequence.Append(target.DOScale(1f, duration / 2).SetEase(Ease.OutQuad));

            await punchSequence.AwaitCompletion();
        }

        public static async Task ShrinkOut(this Transform target, float duration)
        {
            float popDuration = duration / 3;
            float shrinkDuration = popDuration * 2;

            var sequence = DOTween.Sequence();

            sequence.Append(target.DOScale(1.1f, popDuration).SetEase(Ease.OutQuad));
            sequence.Append(target.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InBack));

            await sequence.AwaitCompletion();
        }


        private static Task AwaitCompletion(this Tween tween)
        {
            var tcs = new TaskCompletionSource<bool>();

            tween.OnComplete(() => tcs.TrySetResult(true));
            tween.OnKill(() => tcs.TrySetResult(true));

            return tcs.Task;
        }

    }
}