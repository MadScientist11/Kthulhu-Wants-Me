using System.Linq;
using UnityEngine;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
    public static class DamageExtensions
    {
        private static readonly Collider[] _hitCollidersInternal = new Collider[20];

        public static bool HitFirst<T>(this IDamageSource damageSource, Vector3 startPoint, float radius,
            out T desiredObject)
        {
            D.raw(new Shape.Sphere(startPoint, radius), 1f);

            for (var i = 0; i < _hitCollidersInternal.Length; i++)
            {
                _hitCollidersInternal[i] = null;
            }

            Physics.OverlapSphereNonAlloc(startPoint, radius, _hitCollidersInternal);
            desiredObject = _hitCollidersInternal
                .Where(col =>
                    col != null && col.TryGetComponent(out T _) && col.transform != damageSource.DamageSourceObject)
                .Select(col => col.GetComponent<T>())
                .FirstOrDefault();


            return desiredObject != null;
        }
    }
}