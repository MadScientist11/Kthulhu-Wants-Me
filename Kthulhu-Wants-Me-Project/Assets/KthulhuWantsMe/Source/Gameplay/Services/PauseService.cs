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
            _inputService.GameplayScenario.PauseGame += OnPauseGame;
        }


        public void Dispose()
        {
            _inputService.GameplayScenario.PauseGame -= OnPauseGame;
        }

        private void OnPauseGame()
        {
            if (_gamePaused)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;

            _gamePaused = !_gamePaused;
        }
    }
}