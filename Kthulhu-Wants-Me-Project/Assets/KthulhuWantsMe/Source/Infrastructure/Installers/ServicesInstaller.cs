using KthulhuWantsMe.Source.Infrastructure.Services;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<IResourceManager, ResourcesManager>(Lifetime.Singleton);
        }
    }
}