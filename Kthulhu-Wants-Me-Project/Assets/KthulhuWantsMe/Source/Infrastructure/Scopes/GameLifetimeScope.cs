using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using KthulhuWantsMe.Source.Infrastructure.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private SceneDataProvider _sceneDataProvider;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller(_sceneDataProvider));
            builder.RegisterEntryPoint<GameEntryPoint>();
        }

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