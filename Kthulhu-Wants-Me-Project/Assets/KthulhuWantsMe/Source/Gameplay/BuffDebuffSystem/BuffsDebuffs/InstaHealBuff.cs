using KthulhuWantsMe.Source.Gameplay.Enemies;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs
{
    public class InstaHealBuff : IBuffDebuff
    {
        private float _healAmount;

        public void Init(int healAmount)
        {
            _healAmount = healAmount;
        }

  
        public void ApplyEffect(IEffectReceiver effectReceiver)
        {
            Debug.Log("Heal?");
            if (effectReceiver.Transform.TryGetComponent(out IHealable healable))
            {
                healable.Heal(_healAmount);
            }
        }
    }
}