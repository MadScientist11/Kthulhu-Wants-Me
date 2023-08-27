using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    [RequireComponent(typeof(WeaponBase))]
    public class WeaponItem : PickableItem, IWeapon
    {
        [field: SerializeField] public WeaponData WeaponData { get; set; }
        public override PickableData ItemData => WeaponData;
        
        
    }
}