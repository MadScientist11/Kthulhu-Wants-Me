using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IDamageProvider, IDamageSource
    {
        public Transform DamageSourceObject => transform;

        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerTentacleInteraction _playerTentacleInteraction;
        [SerializeField] private List<Attack> _attackComboSet;

        private IInventorySystem _inventorySystem;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfiguration;
        private IPlayerStats _playerStats;

        private bool _queuedAttack;
        private int _comboAttackIndex;


        [Inject]
        public void Construct(IInventorySystem inventorySystem, IInputService inputService, IDataProvider dataProvider,
            IPlayerStats playerStats)
        {
            _playerStats = playerStats;
            _inputService = inputService;
            _inventorySystem = inventorySystem;
            _playerConfiguration = dataProvider.PlayerConfig;
            
            _inputService.GameplayScenario.Attack += PerformAttack;

        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Attack -= PerformAttack;
        }

        public float ProvideDamage()
        {
            return _playerConfiguration.BaseDamage;
        }

        private void PerformAttack()
        {
            if (_inventorySystem.CurrentItem is not WeaponItem weaponItem)
                return;


            WeaponBase weapon = weaponItem.GetComponent<WeaponBase>();
            PerformHit(weapon);
        }

        private void PerformHit(WeaponBase weapon)
        {
            if (_playerAnimator.IsAttacking && !_playerTentacleInteraction.PlayerGrabbed)
            {
                _queuedAttack = true;
                return;
            }

            _playerAnimator.PlayAttack(_attackComboSet[_comboAttackIndex].AttackOverrideController);
        }

        private void OnAttack()
        {
            D.raw(new Shape.Sphere(AttackStartPoint(), _playerConfiguration.AttackRadius), 1f);

            if (!this.HitFirst(AttackStartPoint(), _playerConfiguration.AttackRadius, out IDamageable damageable))
                return;

            damageable.TakeDamage(_playerStats.ProvideDamage() + _attackComboSet[_comboAttackIndex].Damage);
        }

        private void OnAttackEnd()
        {
            if (_queuedAttack)
            {
                _queuedAttack = false;
                _comboAttackIndex++;
                _comboAttackIndex %= _attackComboSet.Count;
                PerformAttack();
                return;
            }

            _comboAttackIndex = 0;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _playerConfiguration.AttackEffectiveDistance;
        }
    }
}