﻿using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerSpecialAttackAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private MMFeedbacks _specialAttackFeedback;
        
        private WeaponItem _currentWeapon;

        private IInputService _inputService;
        private ThePlayer _player;

        [Inject]
        public void Construct(IInputService inputService, ThePlayer player)
        {
            _player = player;
            _inputService = inputService;

            _inputService.GameplayScenario.SpecialAttack += PerformSpecialAttack;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.SpecialAttack -= PerformSpecialAttack;
        }

        private void OnSpecialAttack()
        {
            _currentWeapon.GetComponent<ISpecialAttack>().RespondTo(this);
        }

        private void PerformSpecialAttack()
        {
            if(GetComponent<PlayerLungeAbility>().IsInLunge)
                return;
            
            if (_player.Inventory.CurrentItem is WeaponItem weapon && weapon.WeaponData.WeaponMoveSet.HasSpecialAttack)
            {
                _currentWeapon = weapon;
                GetComponent<PlayerLocomotion>().FaceMouse();
                _playerAnimator.PlaySpecialAttack();
                _specialAttackFeedback?.PlayFeedbacks();
            }
        }
    }
}