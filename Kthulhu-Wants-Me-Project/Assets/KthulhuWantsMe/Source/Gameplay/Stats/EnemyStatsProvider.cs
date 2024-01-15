using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;

namespace KthulhuWantsMe.Source.Gameplay.Stats
{
    public class EnemyStatsProvider
    {
        private readonly EnemyStatsScalingService _enemyStatsScalingService;
        private IDataProvider _dataProvider;

        public EnemyStatsProvider(IDataProvider dataProvider, EnemyStatsScalingService enemyStatsScalingService)
        {
            _dataProvider = dataProvider;
            _enemyStatsScalingService = enemyStatsScalingService;
        }
        
        public EnemyStats StatsFor(EnemyType enemyType, int level)
        {
            return _enemyStatsScalingService.ScaleFor(_dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType], 
                _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType].EnemyScaling, level);
        }
    }
}