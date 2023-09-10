using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class EntityBuffDebuffContainer : MonoBehaviour, IEffectReceiver
    {
        public Transform Transform => transform;

        public List<IBuffDebuff> BuffDebuffs { get; } = new();
        
        
    }
}