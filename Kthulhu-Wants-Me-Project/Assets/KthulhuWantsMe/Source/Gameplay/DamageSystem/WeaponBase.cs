using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
    
    public class WeaponBase : MonoBehaviour, IDamageProvider
    {
        public Transform DamageDealer => transform;
        
        protected List<IDamageProvider> _damageProviders;
        private WeaponItem _weaponItem;


        private void Awake()
        {
            _weaponItem = GetComponent<WeaponItem>();
        }

        public void SetDamageProviders(List<IDamageProvider> damageProviders)
        {
            _damageProviders = damageProviders;
            _damageProviders.Add(this);
        }

        public float ProvideDamage()
        {
            return _weaponItem.WeaponData.BaseDamage;
        }
    }
}