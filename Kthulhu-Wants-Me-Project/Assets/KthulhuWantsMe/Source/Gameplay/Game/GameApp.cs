using System;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Game
{
    public class GameApp : IInitializable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IUIService _uiService;

        public GameApp(IInputService inputService, IUIService uiService)
        {
            _uiService = uiService;
            _inputService = inputService;
        }
        
        public void Initialize()
        {
            _inputService.GameScenario.PauseGame += OnPauseGame;
        }

        public void Dispose()
        {
            _inputService.GameScenario.PauseGame -= OnPauseGame;
        }

        private void OnPauseGame()
        {
            if (_uiService.IsOpen(WindowId.PauseWindow))
            {
                _uiService.CloseWindow(WindowId.PauseWindow);
            }
            else
            {
                _uiService.OpenWindow(WindowId.PauseWindow);
            }
        }

    }
}