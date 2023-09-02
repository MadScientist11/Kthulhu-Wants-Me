using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => _tentacleConfig.MaxHealth;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _tentacleConfig = dataProvider.TentacleConfig;

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            DoDamageImpact();
        }

        private void DoDamageImpact() => 
            _tentacleAnimator.PlayImpact();
    }
}