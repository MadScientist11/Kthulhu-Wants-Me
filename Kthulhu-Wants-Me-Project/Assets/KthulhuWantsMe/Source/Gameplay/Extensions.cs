using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;
using Random = System.Random;

namespace KthulhuWantsMe.Source.Gameplay
{
    public static class Extensions
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElementUsing<T>(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
        
        public static void SetLayer<T>(this GameObject gameobject, int layer, bool includeChildren = false)
            where T : Component
        {
            gameobject.layer = layer;
            if (includeChildren == false) return;

            var arr = gameobject.GetComponentsInChildren<T>(true);

            for (int i = 0; i < arr.Length; i++)
                arr[i].gameObject.layer = layer;
        }

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

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}