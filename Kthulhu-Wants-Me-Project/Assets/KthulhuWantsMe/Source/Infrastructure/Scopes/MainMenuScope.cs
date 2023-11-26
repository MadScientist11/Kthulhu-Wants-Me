using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class MainMenuScope : LifetimeScope
    {
#if UNITY_EDITOR
        [Button]
        public void CollectInjectables()
        {
            IEnumerable<MonoBehaviour> interactables =
                FindObjectsOfType<MonoBehaviour>().Where(mb => mb.TryGetComponent(out IInjectable _));
            autoInjectGameObjects = interactables.Select(injectable => injectable.gameObject).ToHashSet().ToList();
        }
#endif
    }
}