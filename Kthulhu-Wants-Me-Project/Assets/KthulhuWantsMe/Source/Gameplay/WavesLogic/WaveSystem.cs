using System.Collections.Generic;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public class WaveSystem
    {
        private Waves _wavesData;
        private IGameFactory _gameFactory;
        private IProgressService _progressService;
        private Location _location;

        public WaveSystem(IDataProvider dataProvider, IGameFactory gameFactory, IProgressService progressService,
            Location location)
        {
            _location = location;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void StartNextWave()
        {
            List<WaveEnemy> waveEnemies = _wavesData.WaveData[_progressService.ProgressData.WaveIndex].WaveEnemies;
            SpawnWaveEnemies(waveEnemies);
        }

        private void SpawnWaveEnemies(List<WaveEnemy> waveEnemies)
        {
            foreach (WaveEnemy waveEnemy in waveEnemies)
            {
                for (int i = 0; i < waveEnemy.Quantity; i++)
                {
                    EnemySpawnZoneData spawnZone = _location.EnemySpawnZones[Random.Range(0, _location.PortalSpawnZones.Count)];
                    Vector3 randomPoint = Mathfs.Abs(Random.insideUnitSphere * spawnZone.Radius);

                    Vector3 spawnPosition =
                        spawnZone.Position +
                        randomPoint;
                    if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100))
                    {
                        Portal portal =
                            _gameFactory.CreatePortalWithEnemy(hitInfo.point + Vector3.one* 0.05f, Quaternion.identity, waveEnemy.EnemyType);
                        GameObject enemy = portal.LastSpawnedEnemy;
                    }
                }
            }
        }
    }
}