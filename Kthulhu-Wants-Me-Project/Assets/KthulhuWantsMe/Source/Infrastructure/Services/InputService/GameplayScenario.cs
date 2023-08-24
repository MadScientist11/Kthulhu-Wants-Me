using UnityEngine;
using UnityEngine.InputSystem;

namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public class GameplayScenario : GameInput.IGameplayActions, IInputScenario
    {
        private GameInput.GameplayActions _gameplayActions;
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public GameplayScenario(GameInput.GameplayActions gameplayActions)
        {
            _gameplayActions = gameplayActions;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void Enable() =>
            _gameplayActions.Enable();

        public void Disable() =>
            _gameplayActions.Disable();
    }
}