using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
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