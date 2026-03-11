using UnityEngine;

namespace Blast.Data
{
    public static class DataHelper
    {
        public static bool TryCast<T>(IData data, out T typedData) where T : class, IData
        {
            if (data is T validData)
            {
                typedData = validData;

                return true;
            }
            else
            {
                Debug.LogError($"Invalid data type! " +
                    $"Expected {nameof(BlockData)}, " +
                    $"got {data?.GetType().Name ?? "null"}");

                typedData = null;
                return false;
            }
        }
    }
}