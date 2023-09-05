using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.SOData
{
    public enum WeaponType
    {
        Throwable = 0,
        Hitable = 1,
    }
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create WeaponData", fileName = "WeaponData", order = 0)]
    public class WeaponData : PickableData
    {
        public WeaponType WeaponType;
        public float BaseDamage;

        public bool HasSpecialAttack => SpecialAttack != null;

        public Attack SpecialAttack;


    }
}