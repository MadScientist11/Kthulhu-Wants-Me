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
        bool SomeonesAttacking { get; set; }
        int EnemiesCount { get; }
        bool AllowedChasingPlayer(GameObject enemy);
        void AddToChase(GameObject enemy);
        bool CanAttack();
    }

    public class EnemiesAIBrainService : IAIService, IInitializable, ITickable
    {
        public bool SomeonesAttacking { get; set; }

        public int EnemiesCount
        {
            get { return _waveSystemDirector.CurrentWaveState.AliveEnemies.Count; }
        }

        private List<GameObject> _chasingPlayerEnemies;
        private float _behaviourRevaluationCooldown;

        private readonly IGameFactory _gameFactory;
        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly IResourceManager _resourceManager;
        private GlobalAIConfiguration _globalAIConfiguration;

        public EnemiesAIBrainService(IGameFactory gameFactory, IResourceManager resourceManager,
            IWaveSystemDirector waveSystemDirector)
        {
            _resourceManager = resourceManager;
            _waveSystemDirector = waveSystemDirector;
            _gameFactory = gameFactory;
        }

        public async void Initialize()
        {
            _globalAIConfiguration =
                (GlobalAIConfiguration)await _resourceManager.ProvideAssetAsync<GlobalAIConfiguration>(
                    "AI/GlobalAIConfig");
            _chasingPlayerEnemies = new List<GameObject>(_globalAIConfiguration.MaxChasingEnemies);
        }

        public void Tick()
        {
            if (_waveSystemDirector.CurrentWaveState == null)
                return;

            _behaviourRevaluationCooldown -= Time.deltaTime;

            if (_behaviourRevaluationCooldown <= 0)
            {
                ReevaluateEnemiesBehaviour();
                _behaviourRevaluationCooldown = _globalAIConfiguration.AIBehaviourUpdateRate;
            }
        }

        public bool AllowedChasingPlayer(GameObject enemy)
        {
            return _chasingPlayerEnemies.Contains(enemy);
        }

        public bool CanAttack()
        {
            return !SomeonesAttacking;
        }
        
        public void AddToChase(GameObject enemy)
        {
            if (_chasingPlayerEnemies.Contains(enemy))
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

        private void ReevaluateEnemiesBehaviour()
        {
            _chasingPlayerEnemies.RemoveAll(item => item == null);
            RemoveEnemiesThatFallBehind();
            IEnumerable<GameObject> closestEnemies = _waveSystemDirector.CurrentWaveState.AliveEnemies
                .Where(enemyHealth => !_chasingPlayerEnemies.Contains(enemyHealth.gameObject))
                .OrderBy(enemyHealth =>
                    Vector3.Distance(enemyHealth.transform.position, _gameFactory.Player.transform.position))
                .Take(_globalAIConfiguration.MaxChasingEnemies - _chasingPlayerEnemies.Count)
                .Select(enemy => enemy.gameObject);

            _chasingPlayerEnemies.AddRange(closestEnemies);
        }

        private void RemoveEnemiesThatFallBehind()
        {
            for (var i = 0; i < _chasingPlayerEnemies.Count; i++)
            {
                if (Vector3.Distance(_chasingPlayerEnemies[i].transform.position,
                        _gameFactory.Player.transform.position) > _globalAIConfiguration.EnemiesFallBehindDistance)
                {
                    _chasingPlayerEnemies.RemoveAt(i);
                }
            }
        }
    }
}