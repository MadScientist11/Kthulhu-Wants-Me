using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class MainMenuScope : DefaultInjectionScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuEntryPoint>();
        }
    }
}