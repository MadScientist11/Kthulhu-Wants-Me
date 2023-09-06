using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create BuffData", fileName = "BuffData", order = 0)]
    public class BuffData : ScriptableObject
    {
        public GameObject BuffPrefab;
        public BuffTarget BuffTarget;
        public BuffType BuffType;
        public float Value;
    }
}