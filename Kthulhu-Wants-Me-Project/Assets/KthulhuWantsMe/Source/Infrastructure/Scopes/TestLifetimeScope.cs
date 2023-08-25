using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class TestLifetimeScope : LifetimeScope
    {
        public PlayerSpawnPoint PlayerSpawnPoint;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller(PlayerSpawnPoint));
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
