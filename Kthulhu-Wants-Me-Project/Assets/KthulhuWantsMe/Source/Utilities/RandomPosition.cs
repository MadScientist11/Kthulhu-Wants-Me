using UnityEngine;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Utilities
{
    public static class RandomPosition
    {
        public static Vector3 GetRandomPosition(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}