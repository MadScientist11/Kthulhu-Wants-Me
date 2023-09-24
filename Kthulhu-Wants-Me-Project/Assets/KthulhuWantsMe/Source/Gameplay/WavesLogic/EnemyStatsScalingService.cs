using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using org.matheval;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public class EnemyStatsScalingService
    {
       
        public EnemyStats ScaleFor<TConfig>(TConfig config, EnemyScaling enemyScaling, int level) where TConfig : EnemyConfiguration
        {
            EnemyStats enemyStats = new();
            enemyStats.Stats = new();
            foreach ((StatType statType, float baseValue) in config.BaseStats)
            {
                if (enemyScaling.StatsScaling.TryGetValue(statType, out ScaleParameter scale))
                {
                    Expression formula = new Expression(scale.Formula);
                    formula.Bind("base", baseValue);
                    formula.Bind("x", level);
                    enemyStats.Stats[statType] = (float)formula.Eval<decimal>();
                }
                else
                {
                    enemyStats.Stats[statType] = config.BaseStats[statType];
                }
            }
            
           
            return enemyStats;
        }
    }
}