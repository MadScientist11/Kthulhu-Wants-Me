using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren = false)
        {
            gameObject.layer = layer;
            
            if (includeChildren == false) return;

            foreach (Transform child in gameObject.transform)
                child.gameObject.layer = layer;
        }
        
        public static void SwitchOn(this GameObject go)
        {
            go.SetActive(true);
        }

        public static void SwitchOff(this GameObject go)
        {
            go.SetActive(false);
        }
    }
}