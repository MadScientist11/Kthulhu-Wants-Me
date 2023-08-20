using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class AppLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Enqueue(new ServicesInstaller());
            builder.RegisterEntryPoint<Boot>();
        }
    }
}
