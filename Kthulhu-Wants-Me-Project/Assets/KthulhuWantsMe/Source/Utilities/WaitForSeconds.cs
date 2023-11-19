using System.Collections.Generic;

namespace KthulhuWantsMe.Source.Utilities
{
    public static class WaitForSeconds
    {
        private static readonly Dictionary<float, UnityEngine.WaitForSeconds>
            WaitDictionary = new();

        public static UnityEngine.WaitForSeconds Wait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out UnityEngine.WaitForSeconds wait))
                return wait;

            WaitDictionary[time] = new UnityEngine.WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }
}