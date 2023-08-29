using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay
{
    public static class Extensions
    {
        public static bool IsDamageable(this Collider obj, out IDamageable damageable)
        {
            return obj.TryGetComponent(out damageable);
        }
        
        public static bool IsWeapon(this IPickable item, out IDamageProvider damageProvider)
        {
            if (item is not IWeapon || !item.Transform.TryGetComponent(out damageProvider))
            {
                damageProvider = null;
                return false;
            }
            return true;
        }
    }
}
