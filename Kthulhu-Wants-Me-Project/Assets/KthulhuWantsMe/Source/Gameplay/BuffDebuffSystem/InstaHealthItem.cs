using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class InstaHealthItem : BuffItem
    {
        [SerializeField] private int _healAmount;


        protected override IBuffDebuff ProvideBuff()
        {
            InstaHealBuff effect = EffectFactory.CreateEffect<InstaHealBuff>();
            effect.Init(_healAmount);
            return effect;
        }
    }
}