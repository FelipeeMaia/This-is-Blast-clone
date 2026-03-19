using Blast.Interfaces;
using UnityEngine;

namespace Blast.Data
{
    /// <summary>
    /// Helper class with functions to deal with data.
    /// </summary>
    public static class DataHelper
    {
        public static bool TryCast<T>(ISpawnData data, out T typedData) where T : struct, ISpawnData
        {
            if (data is T validData)
            {
                typedData = validData;

                return true;
            }
            else
            {
                Debug.LogError($"Invalid data type! " +
                    $"Expected {typeof(T).Name}, " +
                    $"got {data?.GetType().Name ?? "null"}");

                typedData = default(T);
                return false;
            }
        }
    }
}