using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class DefaultInjectionScope : LifetimeScope
    {
        private void OnValidate()
        {
            CollectInjectables();
        }

#if UNITY_EDITOR
        [Button]
        public void CollectInjectables()
        {
            IEnumerable<GameObject> interactables = FindObjectsOfType<GameObject>().Where(mb => mb.TryGetComponent(out IInjectable _));
            autoInjectGameObjects = interactables.ToList();
        }
#endif
    }
}
