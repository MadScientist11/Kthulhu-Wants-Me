using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller(_playerSpawnPoint));
            builder.RegisterEntryPoint<GameEntryPoint>();
        }

    }
}
