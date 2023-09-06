using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay
{
    public static class PhysicsUtility
    {
        private static readonly Collider[] _hitCollidersInternal = new Collider[20];

        public static bool HitFirst<T>(Transform source, Vector3 startPoint, float radius, LayerMask layer,
            out T desiredObject)
        {
            D.raw(new Shape.Sphere(startPoint, radius), 1f);

            for (var i = 0; i < _hitCollidersInternal.Length; i++)
            {
                _hitCollidersInternal[i] = null;
            }

            Physics.OverlapSphereNonAlloc(startPoint, radius, _hitCollidersInternal, layer);
            desiredObject = _hitCollidersInternal
                .Where(col => col != null && col.TryGetComponent(out T _))
                .Select(col => col.GetComponent<T>())
                .FirstOrDefault();


            return desiredObject != null;
        }
        
        public static bool HitMany<T>(Vector3 startPoint, float radius, LayerMask layer,
            out List<T> desiredObjects)
        {
            D.raw(new Shape.Sphere(startPoint, radius), 1f);

            for (var i = 0; i < _hitCollidersInternal.Length; i++)
            {
                _hitCollidersInternal[i] = null;
            }

            Physics.OverlapSphereNonAlloc(startPoint, radius, _hitCollidersInternal, layer);
            desiredObjects = _hitCollidersInternal
                .Where(col => col != null && col.TryGetComponent(out T _))
                .Select(col => col.GetComponent<T>())
                .ToList();


            return desiredObjects != null && desiredObjects.Count > 0;
        }

        public static bool HitFirst<T>(Transform source, Vector3 startPoint, float radius,
            Func<T, bool> wherePredicate,
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
                    col != null && col.TryGetComponent(out T _) && col.transform != source)
                .Select(col => col.GetComponent<T>())
                .FirstOrDefault(wherePredicate.Invoke);


            return desiredObject != null;
        }
    }
}