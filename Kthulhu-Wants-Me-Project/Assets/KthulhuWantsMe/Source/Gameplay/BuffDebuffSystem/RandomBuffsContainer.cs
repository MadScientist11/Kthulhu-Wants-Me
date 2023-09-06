using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create RandomBuffs", fileName = "RandomBuffs", order = 0)]
    public class RandomBuffsContainer : ScriptableObject
    {
        [SerializeField] private List<BuffData> RandomBuffs;
    }
}