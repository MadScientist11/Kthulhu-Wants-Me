using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using UnityEngine.SceneManagement;
using VContainer;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService
    {
        BaseWindow OpenWindow(WindowId windowId);
        MiscUI MiscUI { get; }
        PlayerHUD PlayerHUD { get; }
        void HideHUD();
        void ShowHUD();
        void CloseActiveWindow();
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
        private BaseWindow _activeWindow;
        private WindowId _activeWindowId;

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
            _activeWindow = null;
            switch (windowId)
            {
                case WindowId.UpgradeWindow:
                    _activeWindow = _uiFactory.CreateUpgradeWindow();
                    _activeWindowId = WindowId.UpgradeWindow;
                    return _activeWindow;
                case WindowId.PauseWindow:
                    _activeWindow = _uiFactory.CreatePauseWindow();
                    _activeWindowId = WindowId.PauseWindow;
                    return _activeWindow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
            }
        }

        public void CloseActiveWindow()
        {
            Object.Destroy(_activeWindow.gameObject);
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