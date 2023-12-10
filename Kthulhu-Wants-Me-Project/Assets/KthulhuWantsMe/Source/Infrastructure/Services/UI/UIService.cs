using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.MainMenu.Settings;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using KthulhuWantsMe.Source.UI.PlayerHUD.TooltipSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService
    {
        PlayerHUD PlayerHUD { get; }
        Tooltip Tooltip { get; }
        void InitHUD();
        BaseWindow OpenWindow(WindowId windowId);
        void CloseWindow(WindowId windowId);

        bool IsOpen(WindowId windowId);
        void ClearUI();
    }

    public class UIService : IUIService
    {
        public bool IsInitialized { get; set; }

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
        
        public Tooltip Tooltip
        {
            get
            {
                if (_tooltip == null)
                {
                    _tooltip = _uiFactory.CreateTooltip();
                }

                return _tooltip;
            }
        }

        private PlayerHUD _playerHUD;
        private Tooltip _tooltip;
        private readonly List<BaseWindow> _activeWindows = new();
        private WindowId _activeWindowId;

        private ISceneService _sceneService;
        private IUIFactory _uiFactory;
        private IPauseService _pauseService;
        private IInputService _inputService;


        [Inject]
        public void Construct(ISceneService sceneService, IUIFactory uiFactory, IPauseService pauseService, 
            IInputService inputService)
        {
            _pauseService = pauseService;
            _uiFactory = uiFactory;
            _sceneService = sceneService;
            _inputService = inputService;
        }

        public BaseWindow OpenWindow(WindowId windowId)
        {
            BaseWindow window = windowId switch
            {
                WindowId.UpgradeWindow => _uiFactory.CreateUpgradeWindow(),
                WindowId.PauseWindow => _uiFactory.CreatePauseWindow(),
                WindowId.DefeatWindow => _uiFactory.CreateDefeatWindow(),
                WindowId.SettingsWindow => _uiFactory.CreateSettingsWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null)
            };
            RenderOnTop(window);
            _activeWindows.Add(window);
            
            if (_activeWindows.Count > 0)
            {
                _pauseService.PauseGame();
            }
            else
            {
                _pauseService.ResumeGame();
            }
            
            return window;
        }

        public void CloseWindow(WindowId windowId)
        {
            BaseWindow windowToClose = _activeWindows.FirstOrDefault(window => window.Id == windowId);

            if (windowToClose == null)
            {
                Debug.LogWarning("Window you're trying to clo9se, doesn't exist.");
                return;
            }
            _activeWindows.Remove(windowToClose);
            Object.Destroy(windowToClose.gameObject);

            if (_activeWindows.Count > 0)
            {
                _pauseService.PauseGame();
            }
            else
            {
                _pauseService.ResumeGame();
            }
        }
        

        public bool IsOpen(WindowId windowId) => 
            _activeWindows.FirstOrDefault(window => window.Id == windowId) != null;

        public void ClearUI()
        {
            if(_playerHUD.gameObject != null)
                Object.Destroy(_playerHUD.gameObject);
            
            foreach (WindowId windowId in Enum.GetValues(typeof(WindowId)).Cast<WindowId>())
            {
                CloseWindow(windowId);
            }

        }

        public void InitHUD()
        {
            if (_playerHUD == null)
            {
                _playerHUD = _uiFactory.CreatePlayerHUD();
            }
            _playerHUD.Init();
        }
        
        private void RenderOnTop(BaseWindow window)
        {
            if(_activeWindows.Count == 0)
                return;
            
            int renderOrder = _activeWindows[^1].GetComponent<Canvas>().sortingOrder;
            window.GetComponent<Canvas>().sortingOrder = renderOrder + 1;
        }
    }
}