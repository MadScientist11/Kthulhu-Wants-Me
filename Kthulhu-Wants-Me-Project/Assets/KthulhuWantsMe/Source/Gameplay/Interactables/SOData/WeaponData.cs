using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.SOData
{
    public enum WeaponType
    {
        Throwable = 0,
        Hitable = 1,
    }
    [CreateAssetMenu(menuName = GameConstants.MenuName + "Create WeaponData", fileName = "WeaponData", order = 0)]
    public class WeaponData : PickableData
    {
        public WeaponType WeaponType;
        public int BaseDamage;
    }
}