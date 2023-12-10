using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Weapons
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create WeaponMoveSet", fileName = "WeaponMoveSet", order = 0)]
    public class WeaponMoveSet : ScriptableObject
    {
        [Tooltip("Doesn't include Special Attack")]
        public int MoveSetAttackCount;
        public AnimatorOverrideController MoveSetAnimations;
        public float[] AttackMoveDamage;

        public bool HasSpecialAttack;
        [ShowIf("HasSpecialAttack")]
        public float SpecialAttackDamage;
        
        private void OnValidate()
        {
            if (AttackMoveDamage == null || AttackMoveDamage.Length != MoveSetAttackCount)
            {
                Array.Resize(ref AttackMoveDamage, MoveSetAttackCount);
            }
        }
    }
}