using System;
using System.Collections.Generic;
using System.Linq;
using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
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

        private IInventorySystem _inventorySystem;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfiguration;
        private IPlayerStats _playerStats;

        [Inject]
        public void Construct(IInventorySystem inventorySystem, IInputService inputService, IDataProvider dataProvider, IPlayerStats playerStats)
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
            return 10;
        }

        private void PerformAttack()
        {
            if (_inventorySystem.CurrentItem is not WeaponItem weaponItem)
                return;

            WeaponBase weapon = weaponItem.GetComponent<WeaponBase>();
          
            switch (weaponItem.WeaponData.WeaponType)
            {
                case WeaponType.Throwable:
                    PerformThrow(weapon, weaponItem);
                    break;
                case WeaponType.Hitable:
                    PerformHit(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PerformThrow(WeaponBase weapon, WeaponItem item)
        {
            _inventorySystem.RemoveItemWithoutNotify(item);
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        }

        private void PerformHit(WeaponBase weapon)
        {
            _playerAnimator.PlayAttack();
        }

        private void OnAttack()
        {
            D.raw(new Shape.Sphere(AttackStartPoint(), _playerConfiguration.AttackRadius), 1f);

            if(!Hit(out Collider hitObject))
                return;

            if (hitObject.IsDamageable(out IDamageable damageable))
            {
                damageable.TakeDamage(_playerStats.ProvideDamage());
            }
        }

        private bool Hit(out Collider col)
        {
            Collider[] results = new Collider[1];
            int hitObjectsCount = Physics.OverlapSphereNonAlloc(AttackStartPoint(), _playerConfiguration.AttackRadius, results);
            col = results.FirstOrDefault();
            return hitObjectsCount > 0;
        }

        private Vector3 AttackStartPoint()
        {
           return new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z) + 
                  transform.forward * _playerConfiguration.AttackEffectiveDistance;
        }
    }
}