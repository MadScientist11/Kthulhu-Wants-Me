using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IDamageProvider
    {
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }

        public float ProvideDamage()
        {
            return 10;
        }

        private void Attack()
        {
            if (_inventorySystem.CurrentItem is not WeaponItem weaponItem)
                return;
            
            WeaponBase weapon = weaponItem.GetComponent<WeaponBase>();
            weapon.SetDamageProviders(new List<IDamageProvider>()
            {
                this
            });

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
            
        }
    }
}