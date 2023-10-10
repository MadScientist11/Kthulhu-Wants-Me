using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public class GameScenario : GameInput.IGameActions, IInputScenario
    {
        private GameInput.GameActions _gameActions;
        
        public event Action PauseGame;

        public GameScenario(GameInput.GameActions gameActions)
        {
            _gameActions = gameActions;
        }
        
        public void OnPause(InputAction.CallbackContext context)
        {
            Debug.Log("Pause?");
            if (context.performed)
            {
                PauseGame?.Invoke();
            }
        }


        public void Enable() =>
            _gameActions.Enable();

        public void Disable() =>
            _gameActions.Disable();
    }
}