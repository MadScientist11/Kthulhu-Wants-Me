using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using NaughtyAttributes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Location _location;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller(_location));
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