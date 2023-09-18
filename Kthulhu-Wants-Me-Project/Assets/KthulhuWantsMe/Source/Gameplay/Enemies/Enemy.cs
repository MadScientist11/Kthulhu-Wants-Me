using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public EnemyStats EnemyStats { get; private set; }
        
        public void Initialize(EnemyStats enemyStats)
        {
            EnemyStats = enemyStats;
            Debug.Log($"Enemy {gameObject.name} initialized with {EnemyStats.Stats[StatType.Health]} hp");
        }
    }
}