﻿using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerDashAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private PlayerFacade _player;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private TentacleGrabAbilityResponse _grabAbilityResponse;
        
        private PlayerLocomotion PlayerLocomotion => _player.PlayerLocomotion;
        
        private float _nextDashTime;
        
        private IInputService _inputService;
        private PlayerStats _playerStats;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, ThePlayer player)
        {
            _inputService = inputService;
            _playerStats = player.PlayerStats;
            _playerConfig = dataProvider.PlayerConfig;
            _inputService.GameplayScenario.Dash += PerformDash;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Dash -= PerformDash;
        }

        private void OnDashEnd()
        {
            PlayerLocomotion.AllowInput();
            PlayerLocomotion.MovementController.EnableCollisionDetection();
        }

        private void PerformDash()
        {
            if (CanDash())
            {
                Dash();
                _nextDashTime = Time.time + _playerStats.MainStats[StatType.EvadeCooldown];
            }
        }

        private void Dash()
        {
            PlayerLocomotion.MovementController.DisableCollisionDetection();
            PlayerLocomotion.BlockInput();
            
            _playerAnimator.PlayEvade();
            PlayerLocomotion.MovementController.AddVelocity(transform.forward * _playerConfig.DashStrength);
        }

        private bool CanDash()
        {
            return !_grabAbilityResponse.Grabbed && PlayerLocomotion.IsMoving && Time.time >= _nextDashTime;
        }
    }
}