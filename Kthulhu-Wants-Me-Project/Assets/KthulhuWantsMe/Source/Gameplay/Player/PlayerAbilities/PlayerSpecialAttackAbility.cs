using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerSpecialAttackAbility : MonoBehaviour, IAbility
    {
        
        [SerializeField] private PlayerAnimator _playerAnimator;
        
        private IInputService _inputService;
        private IInventorySystem _inventorySystem;

        private WeaponItem _currentWeapon;

        [Inject]
        public void Construct(IInputService inputService, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _inputService = inputService;

            _inputService.GameplayScenario.SpecialAttack += PerformSpecialAttack;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.SpecialAttack -= PerformSpecialAttack;
        }

        private void OnSpecialAttack()
        {
            Debug.Log("Special attack");
            _currentWeapon.GetComponent<ISpecialAttack>().RespondTo(this);
        }

        private void PerformSpecialAttack()
        {
            if (_inventorySystem.CurrentItem is WeaponItem weapon && weapon.WeaponData.HasSpecialAttack)
            {
                _currentWeapon = weapon;
                _playerAnimator.PlaySpecialAttack(weapon.WeaponData.SpecialAttack.AttackOverrideController);
            }
        }
    }
}