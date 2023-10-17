using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    
    public interface IAIService
    {
        void PickAIBehaviour(GameObject enemyAI);
    }

    public class EnemiesAIBrainService : IAIService, IFixedTickable
    {
        private IGameFactory _gameFactory;
        private IWaveSystemDirector _waveSystemDirector;

        public EnemiesAIBrainService(IGameFactory gameFactory, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _gameFactory = gameFactory;
        }
        public void FixedTick()
        {
            
        }
        
        // Construct closest enemies array, if one of theem is enemyai than make them follow player, if not make them scatter or roam around
        public void PickAIBehaviour(GameObject enemyAI)
        {
            IEnumerable<Health> closestEnemies = _waveSystemDirector.CurrentWaveState.AliveEnemies
                .OrderBy(enemyHealth => Vector3.Distance(enemyHealth.transform.position, _gameFactory.Player.transform.position)).Take(5);

            foreach (Health closestEnemy in closestEnemies)
            {
                if (closestEnemy.gameObject == enemyAI)
                {
                    enemyAI.GetComponent<IEnemyAIBrain>().AssignTask(AITask.FollowPlayer);
                }
                else
                {
                    enemyAI.GetComponent<IEnemyAIBrain>().AssignTask(AITask.None);
                }
            }
        }

       
    }
}
