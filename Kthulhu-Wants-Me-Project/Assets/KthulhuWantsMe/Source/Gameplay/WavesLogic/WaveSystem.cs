using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public class WaveSystem
    {

        private Waves _wavesData;
        private IGameFactory _gameFactory;
        private IProgressService _progressService;

        public WaveSystem(IDataProvider dataProvider, IGameFactory gameFactory, IProgressService progressService)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void StartNextWave()
        {
            List<WaveEnemy> waveEnemies = _wavesData.WaveData[_progressService.ProgressData.WaveIndex].WaveEnemies;
            
        }

        private void SpawnWaveEnemies(List<WaveEnemy> waveEnemies)
        {
            foreach (WaveEnemy waveEnemy in waveEnemies)
            {
                for (int i = 0; i < waveEnemy.Quantity; i++)
                {
                    Portal portal = _gameFactory.CreatePortalWithEnemy(Vector3.zero, Quaternion.identity, waveEnemy.EnemyType);
                    GameObject enemy = portal.LastSpawnedEnemy;
                }
            }
        }

        
        
    }
}
