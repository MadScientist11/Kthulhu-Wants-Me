using KthulhuWantsMe.Source.Infrastructure.Installers;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller());
        }

    }
}
