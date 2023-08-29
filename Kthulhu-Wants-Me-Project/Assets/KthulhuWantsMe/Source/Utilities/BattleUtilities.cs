using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Utilities
{
    public static class BattleUtilities
    {
        private static readonly Collider[] _hitCollidersInternal = new Collider[20];

        public static bool HitFirst(this IDamageSource damageSource, Vector3 startPoint, float radius, out IDamageable damageableObj)
        {
            for (var i = 0; i < _hitCollidersInternal.Length; i++)
            {
                _hitCollidersInternal[i] = null;
            }

            Physics.OverlapSphereNonAlloc(startPoint, radius, _hitCollidersInternal);
            Collider hitCollider = _hitCollidersInternal
                .Where(col => col != null && col.IsDamageable(out IDamageable _))
                .FirstOrDefault(c => c.transform != damageSource.DamageSourceObject);
            
            if (hitCollider == null)
            {
                damageableObj = null;
                return false;
            }
            
            return hitCollider.TryGetComponent(out damageableObj);
        }
    }
}