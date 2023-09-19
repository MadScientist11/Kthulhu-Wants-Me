using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class TestLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
          builder.Install(new GameInstaller());
          builder.RegisterEntryPoint<TestEntryPoint>();
        }
    }
}
