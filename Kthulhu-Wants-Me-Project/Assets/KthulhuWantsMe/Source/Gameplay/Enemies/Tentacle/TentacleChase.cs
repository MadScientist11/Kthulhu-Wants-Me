using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleChase : MonoBehaviour
    {
        private ISpawnBehaviour _spawnBehaviour;

        private float _notClosestSpawnerTime;
        private float _chaseCooldown;

        private TentacleConfiguration _tentacleConfiguration;

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
            _tentacleConfiguration = (TentacleConfiguration)GetComponent<EnemyStatsContainer>().Config;
        }

        private void Update()
        {
            if (_waveSpawner.ClosestSpawner.Id != _spawnBehaviour.SpawnedAt)
            {
                _notClosestSpawnerTime += Time.deltaTime;
            }
            else
            {
                _notClosestSpawnerTime = 0;
            }

            _chaseCooldown -= Time.deltaTime;
        }

        public bool TryRespawn()
        {
            if (_waveSpawner.ClosestSpawner.Id != _spawnBehaviour.SpawnedAt && _waveSpawner.IsSpawnerVacant(_waveSpawner.ClosestSpawner.Id))
            {
                _chaseCooldown = _tentacleConfiguration.ChaseCooldown;
                _waveSpawner.RespawnClosest(_spawnBehaviour.SpawnedAt, GetComponent<Health>(), _tentacleConfiguration.EnemyType);
                return true;
            }

            return false;
        }

        public bool CanChase()
        {
            return enabled && _notClosestSpawnerTime > _tentacleConfiguration.ChaseAfterSeconds && _chaseCooldown <= 0;
        }

     
    }
}