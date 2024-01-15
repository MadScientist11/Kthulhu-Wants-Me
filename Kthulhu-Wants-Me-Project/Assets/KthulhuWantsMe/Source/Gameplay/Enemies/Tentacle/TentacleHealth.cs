using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.Stats;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleRetreat _tentacleRetreat;
        [SerializeField] private MMFeedbacks _deathFeedbacks;

        private void Start()
        {
            Died += OnDie;
        }

        private void OnDestroy()
        {
            Died -= OnDie;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            DoDamageImpact();
        }

        private void DoDamageImpact() => 
            _tentacleAnimator.PlayImpact();

        private void OnDie()
        {
            _tentacleRetreat.RetreatDefeated();
            _deathFeedbacks?.PlayFeedbacks();
        }
    }
}