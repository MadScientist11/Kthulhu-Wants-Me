using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Data
{
    public enum WeaponType
    {
        Throwable = 0,
        Hitable = 1,
    }
    [CreateAssetMenu(menuName = "Create WeaponData", fileName = "WeaponData", order = 0)]
    public class WeaponData : PickableData
    {
        public WeaponType WeaponType;
        public int BaseDamage;
    }
}