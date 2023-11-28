using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Effects;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Player.State;
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
        protected override float BaseDamage => _player.BaseDamage;

        public bool IsAttacking => _isAttacking;
        public bool InRecoveryPhase => _inRecoveryPhase;

        [SerializeField] private MMFeedbacks _attackFeedback;
        [SerializeField] private MMFeedbacks _hitFeedback;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerDashAbility _playerDashAbility;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private TentacleGrabAbilityResponse tentacleGrabAbilityResponse;
        [SerializeField] private DamageModifier _playerDamageModifier;

        private int _comboAttackIndex;
        private WeaponItem _activeWeapon;
        private bool _isAttacking;
        private bool _inRecoveryPhase;

        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private WeaponParticleTrailEffect _weaponTrails;
        private ThePlayer _player;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, ThePlayer player)
        {
            _player = player;
            _inputService = inputService;
            _playerConfiguration = dataProvider.PlayerConfig;

            _inputService.GameplayScenario.Attack += PerformAttack;
            player.Inventory.OnCurrentItemChanged += UpdateActiveWeaponStatus;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Attack -= PerformAttack;
            _player.Inventory.OnCurrentItemChanged -= UpdateActiveWeaponStatus;
        }

        public override float ProvideDamage() =>
            base.ProvideDamage() + _activeWeapon.WeaponData.BaseDamage +
            _activeWeapon.WeaponData.WeaponMoveSet.AttackMoveDamage[_comboAttackIndex];

        public void ResetAttackState(bool resetCombo = true)
        {
            _isAttacking = false;
            _inRecoveryPhase = false;
            

            if (resetCombo)
                _comboAttackIndex = 0;
        }

        private CancellationTokenSource _attackStateResetToken;
        private bool _delayedResetInProgress;

        public async UniTaskVoid ResetAttackStateDelayed()
        {
            if (_delayedResetInProgress)
                return;
            _delayedResetInProgress = true;
            _attackStateResetToken = new();
            await UniTask.Delay(400, false, PlayerLoopTiming.Update, _attackStateResetToken.Token);
            ResetAttackState();
            _delayedResetInProgress = false;
        }

        protected override void OnWindUpPhase()
        {
            _inRecoveryPhase = false;

            Vector3 desiredDirection = _playerLocomotion.FaceMouse();
            _playerLocomotion.MovementController.AddVelocity(desiredDirection * _playerConfiguration.AttackStep);

            //if (Physics.SphereCast(transform.position, 1, transform.forward, out RaycastHit hitInfo, 5,
            //        LayerMasks.EnemyMask))
        }

        protected override void OnContactPhase()
        {
            UpdateActiveWeaponStatus(_player.Inventory.CurrentItem);
            _weaponTrails.Play(_comboAttackIndex);
            _attackFeedback?.PlayFeedbacks(AttackStartPoint());

            if (!PhysicsUtility.HitMany(AttackStartPoint(), _playerConfiguration.AttackRadius,
                    LayerMasks.EnemyMask, out List<IDamageable> damageables))
                return;


            _hitFeedback?.PlayFeedbacks(AttackStartPoint());

            foreach (IDamageable damageable in damageables)
            {
                ApplyDamage(damageable);

                if (damageable.Transform.TryGetComponent(out IEffectReceiver effectReceiver))
                {
                    _playerDamageModifier?.ApplyTo(effectReceiver);
                }
            }
        }

        protected override void OnRecoveryPhase()
        {

            _comboAttackIndex++;
            _comboAttackIndex %= _activeWeapon.WeaponData.WeaponMoveSet.MoveSetAttackCount;
            _inRecoveryPhase = true;
            ResetAttackStateDelayed().Forget();
        }

        protected override void OnAttackEnd()
        {
            ResetAttackState();
        }

        private void PerformAttack()
        {
            if (CantAttack())
            {
                return;
            }

            _attackStateResetToken?.Cancel();
            _delayedResetInProgress = false;
            ResetAttackState(false);
            _playerLocomotion.StopToAttack();
            _playerAnimator.PlayAttack(_comboAttackIndex);
            _isAttacking = true;
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
                _weaponTrails = Instantiate(_activeWeapon.WeaponData.WeaponTrailsPrefab, transform.position, transform.rotation);
            }
            else if (_activeWeapon != null)
            {
                Destroy(_weaponTrails.gameObject);
                _weaponTrails = Instantiate(_activeWeapon.WeaponData.WeaponTrailsPrefab, transform.position, transform.rotation);
            }

            _playerAnimator.ApplyWeaponMoveSet(weaponMoveSet);
        }

        private bool CantAttack() =>
            (_isAttacking && !_inRecoveryPhase)
            || _playerAnimator.CurrentState == AnimatorState.Impact || _playerAnimator.CurrentState == AnimatorState.SpecialAttack
            || tentacleGrabAbilityResponse.Grabbed
            || _activeWeapon == null
            || _playerLocomotion.MovementController.InternalVelocityAdd.sqrMagnitude > 0
            || _playerDashAbility.Dashing;
    }
}