using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public class GameplayScenario : GameInput.IGameplayActions, IInputScenario
    {
        private GameInput.GameplayActions _gameplayActions;
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public event Action<int> SwitchItem; 
        public event Action Attack; 
        public event Action Dash; 
        public event Action GrabResistence; 
       

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

        public void OnSwitchItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SwitchItem?.Invoke((int)context.ReadValue<float>());
            }
                
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack?.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Dash?.Invoke();
            }
        }

        public void OnGrabResistence(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GrabResistence?.Invoke();
            }
        }

        public void Enable() =>
            _gameplayActions.Enable();

        public void Disable() =>
            _gameplayActions.Disable();
    }
}