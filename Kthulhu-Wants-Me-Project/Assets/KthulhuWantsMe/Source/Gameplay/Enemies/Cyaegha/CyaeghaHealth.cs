using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaHealth : Health
    {
        public override float MaxHealth => _enemy.EnemyStats.Stats[StatType.MaxHealth];

        [SerializeField] private Enemy _enemy;
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        
        private void Start()
        {
            Died += HandleDeath;
        }

        private void OnDestroy()
        {
            Died -= HandleDeath;
        }

        private void HandleDeath()
        {
            _hitFeedbacks?.PlayFeedbacks();
            Destroy(gameObject, 2f);
        }
    }
}