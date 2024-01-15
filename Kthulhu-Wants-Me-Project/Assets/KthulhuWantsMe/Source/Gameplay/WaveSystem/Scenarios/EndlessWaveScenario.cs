using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Utilities.Extensions;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Scenarios
{
    public class EndlessWaveScenario : IWaveScenario
    {
        public event Action BatchCleared;

        private readonly EnemyType[] _allowedEnemies = { EnemyType.CyeaghaElite, EnemyType.YithCombo3, EnemyType.TentacleElite };

        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly WaveSpawner _waveSpawner;
        private readonly IGameFactory _gameFactory;
        private readonly IUIService _uiService;

        private CancellationTokenSource _spawnLoopToken;


        public EndlessWaveScenario(IWaveSystemDirector waveSystemDirector, IGameFactory gameFactory,
            IUIService uiService, WaveSpawner waveSpawner)
        {
            _uiService = uiService;
            _gameFactory = gameFactory;
            _waveSpawner = waveSpawner;
            _waveSystemDirector = waveSystemDirector;
        }

        public void Initialize()
        {
            _waveSystemDirector.CurrentWaveState.CurrentWaveData.Batches.Clear();

            AddBatches(1);
            _waveSpawner.SpawnBatchNotified(_waveSystemDirector.CurrentWaveState.CurrentBatchData);
            SpawnBatchLoop().Forget();
            StartObjectiveTimer().Forget();

            _uiService.PlayerHUD._timerUI.gameObject.SwitchOn();
        }

        public void Dispose()
        {
            _uiService.PlayerHUD._timerUI.gameObject.SwitchOff();
        }


        private async UniTaskVoid SpawnBatchLoop()
        {
            _spawnLoopToken = new();
            _spawnLoopToken.RegisterRaiseCancelOnDestroy(_gameFactory.Player);
            while (!_spawnLoopToken.IsCancellationRequested)
            {
                TimeSpan nextBatchDelay =
                    TimeSpan.FromSeconds(_waveSystemDirector.CurrentWaveState.CurrentBatchData.NextBatchDelay);
                await UniTask.Delay(nextBatchDelay, false, PlayerLoopTiming.Update, _spawnLoopToken.Token);
                AddBatches(1);
                _waveSystemDirector.SpawnNextBatch();
            }
        }

        private async UniTaskVoid StartObjectiveTimer()
        {
            TimeSpan tick = TimeSpan.FromSeconds(1);
            int countUp = 0;

            while (true)
            {
                await UniTask.Delay(tick, false, PlayerLoopTiming.Update, _gameFactory.Player.destroyCancellationToken);
                countUp++;
                _uiService.PlayerHUD._timerUI.UpdateTImerText(countUp);
            }
        }

        private void AddBatches(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Batch randomBatch = RandomBatch();
                _waveSystemDirector.CurrentWaveState.CurrentWaveData.Batches.Add(randomBatch);
            }
        }

        private Batch RandomBatch()
        {
            List<EnemyType> allowedEnemies = _allowedEnemies.ToList();

            int aliveTentacles = _waveSystemDirector.CurrentWaveState.AliveEnemies.Count(health =>
                health.TryGetComponent(out TentacleAIBrain _));

            if (aliveTentacles >= 4)
            {
                allowedEnemies = new List<EnemyType> { EnemyType.Cyeagha, EnemyType.YithCombo1 };
            }

            EnemyPack enemyPack = new EnemyPack()
            {
                SpawnAt = EnemySpawnerId.Default,
                EnemyType = allowedEnemies[Random.Range(0, allowedEnemies.Count)],
                Quantity = Random.Range(1, 3),
            };

            if (enemyPack.EnemyType.IsTentacleVariant())
            {
                enemyPack.Quantity = 1;
            }

            return new Batch()
            {
                EnemyPack = new List<EnemyPack>()
                {
                    enemyPack
                },
                NextBatchDelay = Random.Range(2, 5),
            };
        }
    }
}