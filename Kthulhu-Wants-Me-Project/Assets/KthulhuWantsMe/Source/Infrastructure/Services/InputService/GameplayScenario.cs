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
        public event Action GrabResistance;
        public event Action LungePerformed;
        public event Action LungeHold;
        public event Action SpecialAttack;
        public event Action Interact;


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
                GrabResistance?.Invoke();
            }
        }

        public void OnLunge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LungeHold?.Invoke();
            }
            
            if (context.canceled)
            {
                LungePerformed?.Invoke();
            }
        }

        public void OnSpecialAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SpecialAttack?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Interact?.Invoke();
            }
        }

        public void Enable() =>
            _gameplayActions.Enable();

        public void Disable() =>
            _gameplayActions.Disable();
    }
}