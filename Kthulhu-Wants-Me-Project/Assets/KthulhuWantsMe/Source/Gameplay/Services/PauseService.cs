using System;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPauseService
    {
        void ResumeGame();
        void PauseGame();
    }

    public class PauseService : IPauseService, IInitializable, IDisposable
    {
        private bool _gamePaused;
        private readonly IInputService _inputService;
        private IUIService _uiService;

        public PauseService(IInputService inputService, IUIService uiService)
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

        public void PauseGame()
        {
            Time.timeScale = 0;
            _inputService.ActiveScenario.Disable();
            _gamePaused = true;
        }
        
        public void ResumeGame()
        {
            _inputService.ActiveScenario.Enable();
            Time.timeScale = 1;
            _gamePaused = false;
        }

        private void OnPauseGame()
        {
            if (_gamePaused)
            {
                ResumeGame();
                _uiService.CloseActiveWindow();
            }
            else
            {
                PauseGame();
                _uiService.OpenWindow(WindowId.PauseWindow);
            }
        }
    }
}