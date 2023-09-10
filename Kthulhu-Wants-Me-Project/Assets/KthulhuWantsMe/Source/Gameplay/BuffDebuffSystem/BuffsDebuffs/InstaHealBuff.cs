using KthulhuWantsMe.Source.Gameplay.Enemies;

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
            if (effectReceiver.Transform.TryGetComponent(out IHealable healable))
            {
                healable.Heal(_healAmount);
            }
        }
    }
}