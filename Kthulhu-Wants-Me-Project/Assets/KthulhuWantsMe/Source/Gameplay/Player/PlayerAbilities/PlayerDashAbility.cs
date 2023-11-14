﻿using System;
using System.Collections.Generic;
using Freya;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
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
        public bool Dashing => !_stopDashMovement;
        
        [SerializeField] private PlayerFacade _player;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private TentacleGrabAbilityResponse _grabAbilityResponse;

        private PlayerLocomotion PlayerLocomotion => _player.PlayerLocomotion;

        private float _nextDashTime;
        private bool _stopDashMovement = true;

        private IInputService _inputService;
        private PlayerStats _playerStats;
        private ThePlayer _thePlayer;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, ThePlayer player)
        {
            _inputService = inputService;
            _playerStats = player.PlayerStats;
            _playerConfig = dataProvider.PlayerConfig;
            _thePlayer = player;
            _inputService.GameplayScenario.Dash += PerformDash;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Dash -= PerformDash;
        }

        private void OnAnimatorMove()
        {
            AnimatorStateInfo stateInfo = _playerAnimator.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.shortNameHash == PlayerAnimator.Evade && !_stopDashMovement)
            {
                Vector2 movementDirection = Vector2.zero;

                if (_inputService.GameplayScenario.MovementInput.magnitude > Mathfs.Epsilon)
                {
                    movementDirection =
                        -PlayerLocomotion.GetMovementDirection(_inputService.GameplayScenario.MovementInput);
                }
                else if (PlayerLocomotion.LastLookDirection.magnitude > Mathfs.Epsilon)
                {
                    movementDirection = PlayerLocomotion.LastLookDirection.XZ();
                }
                else
                {
                    movementDirection = transform.forward.XZ();
                }

                PlayerLocomotion.MovementController.SetInputs(movementDirection, PlayerLocomotion.LastLookDirection);
            }
        }

        private void OnDashRecovery()
        {
            ResetEvade();
        }

        private void OnDashEnd()
        {
            ResetEvade();
        }

        public void ResetEvade()
        {
            _inputService.GameplayScenario.Enable();
            _player.ChangePlayerLayer(LayerMask.NameToLayer(GameConstants.Layers.Player));
            _stopDashMovement = true;
            PlayerLocomotion.MovementController.ResetInputs();
            PlayerLocomotion.MovementController.ResetSpeedOverride();
        }

        private void PerformDash()
        {
            if (CanDash())
            {
                _playerAttack.ResetAttackState();
                Dash();
                _nextDashTime = Time.time + _playerStats.MainStats[StatType.EvadeCooldown];
            }
        }

        private void Dash()
        {
            _stopDashMovement = false;
            //_inputService.GameplayScenario.Disable();
            PlayerLocomotion.MovementController.ResetInputs();
            PlayerLocomotion.MovementController.OverrideMoveSpeed(_playerConfig.DashSpeed);
            _player.ChangePlayerLayer(LayerMask.NameToLayer(GameConstants.Layers.PlayerRoll));

            _playerAnimator.PlayEvade();
            _thePlayer.ModifyCurrentStamina(-1000);
            //PlayerLocomotion.MovementController.AddVelocity(transform.forward * 10);
        }

        private bool CanDash()
        {
            return !_grabAbilityResponse.Grabbed && IsStaminaFull();
        }

        private bool IsStaminaFull()
        {
            return (int)_thePlayer.PlayerStats.CurrentStamina == (int)_thePlayer.MaxStamina;
        }
    }
}