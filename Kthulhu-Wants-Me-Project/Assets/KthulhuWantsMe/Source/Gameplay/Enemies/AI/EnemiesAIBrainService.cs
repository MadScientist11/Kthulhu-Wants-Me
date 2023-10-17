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
        bool AllowedChasingPlayer(GameObject enemy);
        void AddToChase(GameObject enemy);
    }

    public class EnemiesAIBrainService : IAIService, ITickable
    {
        private List<GameObject> _chasingPlayerEnemies = new List<GameObject>(15);
        
        private IGameFactory _gameFactory;
        private IWaveSystemDirector _waveSystemDirector;

        public EnemiesAIBrainService(IGameFactory gameFactory, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _gameFactory = gameFactory;
        }
        private float _behaviourReavaluationTime = 2f;
        private float _behaviourReavaluationCooldown;
        
        public void Tick()
        {
            if(_waveSystemDirector.CurrentWaveState == null)
                return;
            
            _behaviourReavaluationCooldown -= Time.deltaTime;
            
            if (_behaviourReavaluationCooldown <= 0)
            {
                ReevaluateEnemiesBehaviour();
                _behaviourReavaluationCooldown = _behaviourReavaluationTime;
            }
        }
        
        // Construct closest enemies array, if one of theem is enemyai than make them follow player, if not make them scatter or roam around

        public bool AllowedChasingPlayer(GameObject enemy)
        {
            return _chasingPlayerEnemies.Contains(enemy);
        }

        public void AddToChase(GameObject enemy)
        {
            if(_chasingPlayerEnemies.Contains(enemy))
                return;
            _chasingPlayerEnemies.RemoveAll(item => item == null);

            

            GameObject furthestEnemy = _chasingPlayerEnemies.OrderBy(enemyHealth =>
                    Vector3.Distance(enemyHealth.transform.position, _gameFactory.Player.transform.position))
                .LastOrDefault();

            if (furthestEnemy != null)
            {
                _chasingPlayerEnemies.Remove(furthestEnemy);
                _chasingPlayerEnemies.Add(enemy);
            }
        }
        
        public void ReevaluateEnemiesBehaviour()
        {
            _chasingPlayerEnemies.RemoveAll(item => item == null);
            RemoveEnemiesThatFallBehind();
            IEnumerable<GameObject> closestEnemies = _waveSystemDirector.CurrentWaveState.AliveEnemies.Where(enemyHealth => !_chasingPlayerEnemies.Contains(enemyHealth.gameObject))
                .OrderBy(enemyHealth =>
                    Vector3.Distance(enemyHealth.transform.position, _gameFactory.Player.transform.position)).Take(15 - _chasingPlayerEnemies.Count).Select(enemy => enemy.gameObject);
          
            _chasingPlayerEnemies.AddRange(closestEnemies);
            
            
        }
        
        

        private void RemoveEnemiesThatFallBehind()
        {
            for (var i = 0; i < _chasingPlayerEnemies.Count; i++)
            {
                if (Vector3.Distance(_chasingPlayerEnemies[i].transform.position,
                        _gameFactory.Player.transform.position) > 10)
                {
                    _chasingPlayerEnemies.RemoveAt(i);
                }
            }
        }

        

       
    }
}
