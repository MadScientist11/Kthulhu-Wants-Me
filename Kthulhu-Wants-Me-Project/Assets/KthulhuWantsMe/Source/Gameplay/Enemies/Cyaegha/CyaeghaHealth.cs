using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private Collider _collider;
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
            _collider.enabled = false;
            _hitFeedbacks?.PlayFeedbacks();
            Destroy(gameObject, 2f);
        }
    }
}