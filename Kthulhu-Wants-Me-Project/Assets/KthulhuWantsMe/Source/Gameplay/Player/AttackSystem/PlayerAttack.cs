using System.Collections;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Effects;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.Weapons;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.AttackSystem
{
    public class PlayerAttack : Entity.Attack
    {
        protected override float BaseDamage => _playerStats.BaseDamage;

        public bool IsAttacking => _isAttacking;


        public MMFeedbacks TargetFeedbacks;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private TentacleGrabAbilityResponse tentacleGrabAbilityResponse;
        [SerializeField] private DamageModifier _playerDamageModifier;

        private int _comboAttackIndex;
        private WeaponItem _activeWeapon;
        private bool _isAttacking;
        private bool _canProceedWithCombo;

        private PlayerConfiguration _playerConfiguration;
        private Stats _playerStats;
        private IInputService _inputService;
        private IInventorySystem _inventorySystem;
        private WeaponParticleTrailEffect _weaponTrails;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, IPlayerStats playerStats,
            IInventorySystem inventorySystem)
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
            base.ProvideDamage() + _activeWeapon.WeaponData.BaseDamage +
            _activeWeapon.WeaponData.WeaponMoveSet.AttackMoveDamage[_comboAttackIndex];

        protected override void OnWindUpPhase()
        {
            _isAttacking = true;
            _canProceedWithCombo = false;
            _playerLocomotion.FaceMouse();
        }

        protected override void OnContactPhase()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _playerConfiguration.AttackRadius,
                    LayerMasks.EnemyMask, out IDamageable damageable))
                return;


            _weaponTrails.Play(_comboAttackIndex);
            TargetFeedbacks.PlayFeedbacks(AttackStartPoint());
            ApplyDamage(damageable);
            if (damageable.Transform.TryGetComponent(out IEffectReceiver effectReceiver))
            {
                _playerDamageModifier?.ApplyTo(effectReceiver);

            }
        }

        protected override void OnRecoveryPhase()
        {
            _canProceedWithCombo = true;
            _comboAttackIndex++;
            _comboAttackIndex %= _activeWeapon.WeaponData.WeaponMoveSet.MoveSetAttackCount;
        }

        protected override void OnAttackEnd()
        {
            ResetAttackState();
        }

        private void PerformAttack()
        {
            if (CantAttack())
                return;


            _playerAnimator.PlayAttack(_comboAttackIndex);
        }

        private void ResetAttackState()
        {
            _isAttacking = false;
            _canProceedWithCombo = false;
            _comboAttackIndex = 0;
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
            if (_activeWeapon != null && _weaponTrails == null)
            {
                _weaponTrails = Instantiate(_activeWeapon.WeaponData.WeaponTrailsPrefab, transform);
            }
            else if (_activeWeapon != null)
            {
                Destroy(_weaponTrails);
                _weaponTrails = Instantiate(_activeWeapon.WeaponData.WeaponTrailsPrefab, transform);
            }

            _playerAnimator.ApplyWeaponMoveSet(weaponMoveSet);
        }

        private bool CantAttack() =>
            (_isAttacking && !_canProceedWithCombo)
            || _playerAnimator.CurrentState == AnimatorState.Impact
            || tentacleGrabAbilityResponse.Grabbed
            || _activeWeapon == null
            || _playerLocomotion.MovementController.InternalVelocityAdd.sqrMagnitude > 0;
    }
}