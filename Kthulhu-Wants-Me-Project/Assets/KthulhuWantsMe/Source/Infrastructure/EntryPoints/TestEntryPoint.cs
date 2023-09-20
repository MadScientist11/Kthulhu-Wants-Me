using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class TestEntryPoint : IAsyncStartable
    {
        private readonly IReadOnlyList<IInitializableService> _services;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;

        public TestEntryPoint(IReadOnlyList<IInitializableService> services, IGameFactory gameFactory, IInputService inputService)
        {
            _inputService = inputService;
            _gameFactory = gameFactory;
            _services = services;
        }
        public  async UniTask StartAsync(CancellationToken cancellation)
        {
            Debug.Log("Start");
            List<UniTask> initializationTasks = _services.Where(service => service.IsInitialized == false).Select(service => service.Initialize()).ToList();
            await UniTask.WhenAll(initializationTasks);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
            //_gameFactory.CreateEnemy(Vector3.up *2, Quaternion.identity, EnemyType.Minion);
        }
    }
}