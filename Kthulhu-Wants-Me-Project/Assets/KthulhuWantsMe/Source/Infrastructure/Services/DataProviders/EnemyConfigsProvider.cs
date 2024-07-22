using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services.DataProviders
{
    public class EnemyConfigsProvider
    {
        public Dictionary<EnemyType, EnemyConfiguration> EnemyConfigs { get; private set; } = new();

        public void Initialize()
        {
            foreach (EnemyConfiguration enemy in Resources.LoadAll<EnemyConfiguration>("Enemies"))
            {
                EnemyConfigs.Add(enemy.EnemyType, enemy);
            }
        }
    }
}