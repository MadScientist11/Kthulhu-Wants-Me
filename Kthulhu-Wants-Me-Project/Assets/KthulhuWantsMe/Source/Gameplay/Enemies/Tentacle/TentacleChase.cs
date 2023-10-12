using System;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleChase : MonoBehaviour
    {
        private ISpawnBehaviour _spawnBehaviour;
        
        private WaveSpawner _waveSpawner;
        private WaveState _waveState;

        [Inject]
        public void Construct(IWaveSystemDirector waveSystemDirector)
        {
            _waveSpawner = waveSystemDirector.WaveSpawner;
            _waveState = waveSystemDirector.CurrentWaveState;
        }

        private void Start()
        {
            _spawnBehaviour = GetComponent<ISpawnBehaviour>();
        }

        public void TryRespawn()
        {
            if (_waveSpawner.ClosestSpawner.Id != _spawnBehaviour.SpawnedAt)
            {
                if (!_waveState.AliveEnemiesByPlace.ContainsKey(_waveSpawner.ClosestSpawner.Id))
                {
                    _waveSpawner.RespawnClosest(_spawnBehaviour.SpawnedAt, GetComponent<Health>());
                    return;
                }
                
                if (_waveState.AliveEnemiesByPlace[_waveSpawner.ClosestSpawner.Id].All(enemyHealth => enemyHealth.GetType() != typeof(TentacleHealth)))
                {
                    _waveSpawner.RespawnClosest(_spawnBehaviour.SpawnedAt, GetComponent<Health>());
                }
            }
        }
    }
}