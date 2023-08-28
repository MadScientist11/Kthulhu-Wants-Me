using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IDamageProvider
    {
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private List<Attack> _attackComboSet;

        private IInventorySystem _inventorySystem;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfiguration;
        private IPlayerStats _playerStats;

        private bool _queuedAttack;
        private int _comboAttackIndex;
        private Coroutine _comboProcessor;

        [Inject]
        public void Construct(IInventorySystem inventorySystem, IInputService inputService, IDataProvider dataProvider,
            IPlayerStats playerStats)
        {
            _playerStats = playerStats;
            _inputService = inputService;
            _inventorySystem = inventorySystem;
            _playerConfiguration = dataProvider.PlayerConfig;
        }

        private void Start()
        {
            _inputService.GameplayScenario.Attack += PerformAttack;
            _playerAnimator.OnStateExited += OnAttackStateExited;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Attack -= PerformAttack;
            _playerAnimator.OnStateExited -= OnAttackStateExited;
        }

        public float ProvideDamage()
        {
            return 10;
        }

        private void PerformAttack()
        {
            if (_inventorySystem.CurrentItem is not WeaponItem weaponItem)
                return;


            WeaponBase weapon = weaponItem.GetComponent<WeaponBase>();
            PerformHit(weapon);
        }

        private void PerformThrow(WeaponBase weapon, WeaponItem item)
        {
            _inventorySystem.RemoveItemWithoutNotify(item);
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        }

        private void PerformHit(WeaponBase weapon)
        {
            if (_playerAnimator.IsAttacking)
            {
                _queuedAttack = true;
                return;
            }

            Debug.Log(_comboAttackIndex);
            _playerAnimator.PlayAttack(_attackComboSet[_comboAttackIndex].AttackOverrideController);
        }

        private void OnAttack()
        {
            D.raw(new Shape.Sphere(AttackStartPoint(), _playerConfiguration.AttackRadius), 1f);

            if (!Hit(out Collider hitObject))
                return;

            if (hitObject.IsDamageable(out IDamageable damageable))
            {
                Debug.Log(_attackComboSet[_comboAttackIndex].Damage);
                damageable.TakeDamage(_playerStats.ProvideDamage() + _attackComboSet[_comboAttackIndex].Damage);
            }
        }

        private void OnAttackStateExited(AnimatorState state)
        {
            if (state != AnimatorState.Attack)
                return;

            //if (_comboProcessor == null)
                _comboProcessor = StartCoroutine(ProcessComboDelayed());
        }

        private IEnumerator ProcessComboDelayed()
        {
            yield return WaitForAnimatorStateChange();

            if (_queuedAttack)
            {
                _queuedAttack = false;
                _comboAttackIndex++;
                _comboAttackIndex %= _attackComboSet.Count;
                PerformAttack();
                yield break;
            }

            _comboAttackIndex = 0;
        }

        private WaitForSeconds WaitForAnimatorStateChange()
        {
            return new WaitForSeconds(0.5f);
        }

        private bool Hit(out Collider col)
        {
            Collider[] results = new Collider[1];
            int hitObjectsCount =
                Physics.OverlapSphereNonAlloc(AttackStartPoint(), _playerConfiguration.AttackRadius, results);
            col = results.FirstOrDefault();
            return hitObjectsCount > 0;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _playerConfiguration.AttackEffectiveDistance;
        }
    }
}