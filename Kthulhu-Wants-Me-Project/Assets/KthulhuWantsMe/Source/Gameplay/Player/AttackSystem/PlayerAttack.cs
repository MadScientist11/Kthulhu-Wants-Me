using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.Weapons;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.AttackSystem
{
    public class PlayerAttack : Entity.Attack
    {
        protected override float BaseDamage => _playerStats.BaseDamage;

        private bool QueuedAttack
        {
            get => _queuedAttack;
            set
            {
                _queuedAttack = value;
            
            }
        }

        public bool IsAttacking => _isAttacking;


        public MMFeedbacks TargetFeedbacks;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private TentacleGrabAbilityResponse tentacleGrabAbilityResponse;
        
        private bool _queuedAttack;
        private int _comboAttackIndex;
        private WeaponItem _activeWeapon;
        private bool _isAttacking;
        
        private PlayerConfiguration _playerConfiguration;
        private Stats _playerStats;
        private IInputService _inputService;
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, IPlayerStats playerStats, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _inputService = inputService;
            _playerConfiguration = dataProvider.PlayerConfig;
            _playerStats = playerStats.Stats;
            
            _inputService.GameplayScenario.Attack += PerformAttack;
            _playerHealth.TookDamage += ResetAttackState;
            _inventorySystem.OnCurrentItemChanged += UpdateActiveWeaponStatus;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Attack -= PerformAttack;
            _playerHealth.TookDamage -= ResetAttackState;
            _inventorySystem.OnCurrentItemChanged -= UpdateActiveWeaponStatus;
        }

        public override float ProvideDamage() => 
            base.ProvideDamage() + _activeWeapon.WeaponData.BaseDamage + _activeWeapon.WeaponData.WeaponMoveSet.AttackMoveDamage[_comboAttackIndex];
        
        protected override void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _playerConfiguration.AttackRadius,
                    LayerMasks.EnemyMask, out IDamageable damageable))
                return;

            TargetFeedbacks.PlayFeedbacks(AttackStartPoint());
            ApplyDamage(damageable);
        }

        protected override void OnAttackEnd()
        {
            _isAttacking = false;

            if (_queuedAttack)
            {
                _comboAttackIndex++;
                _comboAttackIndex %= _activeWeapon.WeaponData.WeaponMoveSet.MoveSetAttackCount;
                PerformAttack();
                _queuedAttack = false;
            }
            else
            {
                _comboAttackIndex = 0;
            }
                
        }

        private void PerformAttack()
        {
            if (CantAttack())
            {
                if (_isAttacking) 
                    _queuedAttack = true;
                
                return;
            }

            _playerAnimator.PlayAttack(_comboAttackIndex);
            _isAttacking = true;
        }

        private void ResetAttackState()
        {
            _isAttacking = false;
            _queuedAttack = false;
        }


        private Vector3 AttackStartPoint() =>
            new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + 
            transform.forward * _playerConfiguration.AttackEffectiveDistance;

        private void UpdateActiveWeaponStatus(IPickable item)
        {
            if (item is WeaponItem weaponItem)
                _activeWeapon = weaponItem;
            else
                _activeWeapon = null;

            WeaponMoveSet weaponMoveSet = _activeWeapon == null ? null : _activeWeapon.WeaponData.WeaponMoveSet;
            _playerAnimator.ApplyWeaponMoveSet(weaponMoveSet);
        }

        private bool CantAttack() => 
            _isAttacking 
            || tentacleGrabAbilityResponse.Grabbed 
            || _activeWeapon == null 
            || _playerLocomotion.MovementController.InternalVelocityAdd.sqrMagnitude > 0;
    }
}