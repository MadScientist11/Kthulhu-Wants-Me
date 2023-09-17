using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => _enemy.EnemyStats.Stats[StatType.Health];

        [SerializeField] private Enemy _enemy;
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        
        private ILootService _lootService;


        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            DoDamageImpact();
        }

        private void DoDamageImpact() => 
            _tentacleAnimator.PlayImpact();
    }
}