using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerDashAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private PlayerFacade _player;
        [SerializeField] private TentacleGrabAbilityResponse _grabAbilityResponse;

        private PlayerLocomotion PlayerLocomotion => _player.PlayerLocomotion;
        
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider)
        {
            _inputService = inputService;
            _playerConfig = dataProvider.PlayerConfig;

            _inputService.GameplayScenario.Dash += PerformDash;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Dash -= PerformDash;
        }

        private void PerformDash()
        {
            if (CanDash())
                Dash();
        }

        private void Dash()
        {
            PlayerLocomotion.MovementController.AddVelocity(transform.forward * _playerConfig.DashStrength);
        }

        private bool CanDash()
        {
            return !_grabAbilityResponse.Grabbed && PlayerLocomotion.IsMoving;
        }
    }
}