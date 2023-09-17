using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public class EnemyStatsScalingService
    {
       
        public EnemyStats ScaleFor<TConfig>(TConfig config, EnemyScaling enemyScaling, int level) where TConfig : EnemyConfiguration
        {
            EnemyStats enemyStats = new();
            enemyStats.Stats = new();
            foreach ((StatType statType, float value) in config.Stats)
            {
                if (enemyScaling.StatsScaling.TryGetValue(statType, out ScaleParameter scale))
                {
                    enemyStats.Stats[statType] = value * scale.Multiplier * level;
                }
                else
                {
                    enemyStats.Stats[statType] = config.Stats[statType];
                }
            }
            
           
            return enemyStats;
        }
    }
}