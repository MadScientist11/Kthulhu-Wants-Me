using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyStatsContainer : MonoBehaviour
    {
        public EnemyStats EnemyStats { get; private set; }

        public EnemyType EnemyType { get; private set; }
        public EnemyConfiguration Config { get; private set; }
        
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        public void Initialize(EnemyType enemyType, EnemyStats enemyStats)
        {
            EnemyStats = enemyStats;
            EnemyType = enemyType;
            Config = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType];
            Debug.Log($"Enemy {gameObject.name} initialized with {EnemyStats.Stats[StatType.MaxHealth]} hp");
        }
    }
}