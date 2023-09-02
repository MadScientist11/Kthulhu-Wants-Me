﻿using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Player.AttackSystem
{
    public class PlayerAttack : Entity.Attack
    {
        protected override float BaseDamage => _playerConfiguration.BaseDamage;
        
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private TentacleGrabAbilityResponse tentacleGrabAbilityResponse;
        
        private bool _queuedAttack;
        private int _comboAttackIndex;
        private WeaponItem _activeWeapon;

        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _inputService = inputService;
            _playerConfiguration = dataProvider.PlayerConfig;
            
            _inputService.GameplayScenario.Attack += PerformAttack;
        }

        private void OnDestroy() => 
            _inputService.GameplayScenario.Attack -= PerformAttack;

        public override float ProvideDamage() => 
            base.ProvideDamage() + _activeWeapon.WeaponData.BaseDamage + _playerConfiguration.AttackComboSet[_comboAttackIndex].Damage;

        protected override void OnAttack()
        {
            D.raw(new Shape.Sphere(AttackStartPoint(), _playerConfiguration.AttackRadius), 1f);

            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _playerConfiguration.AttackRadius, out IDamageable damageable))
                return;

            ApplyDamage(damageable);
        }

        protected override void OnAttackEnd()
        {
            if (_queuedAttack)
            {
                _queuedAttack = false;
                _comboAttackIndex++;
                _comboAttackIndex %= _playerConfiguration.AttackComboSet.Count;
                PerformAttack();
                return;
            }

            _comboAttackIndex = 0;
        }

        private void PerformAttack()
        {
            _activeWeapon = GetActiveWeapon();
            
            if (CantAttack())
            {
                _queuedAttack = true;
                return;
            }

            AnimatorOverrideController attack = _playerConfiguration.AttackComboSet[_comboAttackIndex].AttackOverrideController;
            _playerAnimator.PlayAttack(attack);
        }
        
        
        private Vector3 AttackStartPoint() =>
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + 
            transform.forward * _playerConfiguration.AttackEffectiveDistance;

        private WeaponItem GetActiveWeapon()
        {
            if (_inventorySystem.CurrentItem is WeaponItem weaponItem)
                return weaponItem;
            
            return null;
        }

        private bool CantAttack() => 
            _playerAnimator.IsAttacking || tentacleGrabAbilityResponse.Grabbed || _activeWeapon == null;
    }
}