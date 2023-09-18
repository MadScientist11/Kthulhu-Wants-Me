using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithHealth : Health
    {
        public override float MaxHealth => _enemy.EnemyStats.Stats[StatType.Health];

        [SerializeField] private Enemy _enemy;
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        
        public void HandleDeath()
        {
            _hitFeedbacks.PlayFeedbacks();
            Destroy(gameObject, 2f);
        }
    }
}