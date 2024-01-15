using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities.Extensions
{
    public static class ComponentExtensions
    {
        public static void SwitchOn(this Behaviour behaviour)
        {
            behaviour.enabled = true;
        }

        public static void SwitchOff(this Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
        
        public static void SwitchOnGameObject(this Behaviour behaviour)
        {
            behaviour.gameObject.SwitchOn();
        }

        public static void SwitchOffGameObject(this Behaviour behaviour)
        {
            behaviour.gameObject.SwitchOff();
        }
    }
}