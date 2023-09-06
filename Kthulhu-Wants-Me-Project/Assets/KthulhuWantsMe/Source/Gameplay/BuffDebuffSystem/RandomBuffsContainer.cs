using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create RandomBuffs", fileName = "RandomBuffs", order = 0)]
    public class RandomBuffsContainer : ScriptableObject
    {
        public BuffData Random => _randomBuffs[Freya.Random.Range(0, _randomBuffs.Count)];
        
        [SerializeField] private List<BuffData> _randomBuffs;
    }
}