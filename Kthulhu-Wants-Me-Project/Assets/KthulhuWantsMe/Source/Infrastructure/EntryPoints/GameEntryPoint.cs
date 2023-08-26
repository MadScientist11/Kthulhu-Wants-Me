using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Infrastructure.Services;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class GameEntryPoint : IAsyncStartable
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IReadOnlyList<IInitializableService> _services;

        public GameEntryPoint(IReadOnlyList<IInitializableService> services, GameStateMachine gameStateMachine)
        {
            _services = services;
            _gameStateMachine = gameStateMachine;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            List<UniTask> initializationTasks = _services.Where(service => service.IsInitialized == false).Select(service => service.Initialize()).ToList();
            await UniTask.WhenAll(initializationTasks);
            _gameStateMachine.SwitchState<StartGameState>();
        }
    }
}