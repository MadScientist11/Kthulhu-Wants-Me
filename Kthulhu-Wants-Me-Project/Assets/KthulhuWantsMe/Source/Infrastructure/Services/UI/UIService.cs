using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using UnityEngine.SceneManagement;
using VContainer;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService
    {
        BaseWindow OpenWindow(WindowId windowId);
        MiscUI MiscUI { get; }
        PlayerHUD PlayerHUD { get; }
        void HideHUD();
        void ShowHUD();
    }

    public class UIService : IUIService
    
    {
        public bool IsInitialized { get; set; }

        public MiscUI MiscUI
        {
            get
            {
                if (_miscUI == null)
                {
                    _miscUI = _uiFactory.CreateMiscUI();
                }

                return _miscUI;
            }
        }
        public PlayerHUD PlayerHUD
        {
            get
            {
                if (_playerHUD == null)
                {
                    _playerHUD = _uiFactory.CreatePlayerHUD();
                }

                return _playerHUD;
            }
        }
        


        private PlayerHUD _playerHUD;
        private MiscUI _miscUI;

        private ISceneLoader _sceneLoader;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }
        
        public BaseWindow OpenWindow(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.UpgradeWindow:
                    UpgradeWindow upgradeWindow = _uiFactory.CreateUpgradeWindow();
                    return upgradeWindow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
            }
        }

        public void ShowHUD()
        {
            if (_playerHUD == null)
            {
                _playerHUD = _uiFactory.CreatePlayerHUD();
            }
            
            _playerHUD.Show();
        }
        
        public void HideHUD()
        {
            _playerHUD?.Hide();
        }
        
    }
}