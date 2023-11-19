using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleRetreat _tentacleRetreat;

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
        }
    }
}