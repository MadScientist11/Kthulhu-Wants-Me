using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay
{
    public static class Extensions
    {
        public static void SetTransparency(this Material material, int cachedColorProperty, float value)
        {
            Color color = material.GetColor(cachedColorProperty);
            color.a = value;
            material.SetColor(cachedColorProperty, color);
        }
        
        public static bool IsTransparent(this Material material)
        {
            return material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        public static Vector3 AddY(this Vector3 vec, float offset)
        {
            return new Vector3(vec.x, vec.y + offset, vec.z);
        }

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

        public static void SwitchOn(this Behaviour behaviour)
        {
            behaviour.enabled = true;
        }

        public static void SwitchOff(this Behaviour behaviour)
        {
            behaviour.enabled = false;
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