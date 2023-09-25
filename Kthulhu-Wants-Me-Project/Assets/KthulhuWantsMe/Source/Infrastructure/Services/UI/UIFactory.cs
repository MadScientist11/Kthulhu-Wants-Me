using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIFactory
    {
        UniTask<PlayerHUD> CreatePlayerHUD();
    }

    public class UIFactory : IUIFactory
    {
        public const string PlayerHUDPath = "PlayerHUD";

        private readonly IResourceManager _resourceManager;
        private readonly IObjectResolver _instantiator;

        public UIFactory(IResourceManager resourceManager, IObjectResolver instantiator)
        {
            _instantiator = instantiator;
            _resourceManager = resourceManager;
        }

        public async UniTask<PlayerHUD> CreatePlayerHUD()
        {
            PlayerHUD prefab = await _resourceManager.ProvideAsset<PlayerHUD>(PlayerHUDPath);
            PlayerHUD playerHUD = LifetimeScope.Find<GameUILifetimeScope>().Container.Instantiate(prefab);
            playerHUD.Initialize();
            return playerHUD;
        }
    }
}