using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class MainMenuEntryPoint : IAsyncStartable
    {
        private readonly IUIFactory _uiFactory;
        private readonly IObjectResolver _resolver;

        public MainMenuEntryPoint(IUIFactory uiFactory, IObjectResolver resolver)
        {
            _resolver = resolver;
            _uiFactory = uiFactory;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            _uiFactory.UseContainer(_resolver);
            return UniTask.CompletedTask;
        }
    }
}