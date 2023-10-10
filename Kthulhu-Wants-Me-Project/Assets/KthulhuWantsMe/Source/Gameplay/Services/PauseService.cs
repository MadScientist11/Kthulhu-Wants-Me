using System;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPauseService
    {
    }

    public class PauseService : IPauseService, IInitializable, IDisposable
    {
        private bool _gamePaused;
        private readonly IInputService _inputService;

        public PauseService(IInputService inputService)
        {
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
            if (_gamePaused)
            {
                _inputService.ActiveScenario.Enable();
                Time.timeScale = 1;
                
            }
            else
            {
                Time.timeScale = 0;
                _inputService.ActiveScenario.Disable();
            }

            _gamePaused = !_gamePaused;
        }
    }
}