using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class StartGameState : IGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly Location _location;
        private readonly IPortalSystem _portalSystem;

        public StartGameState(GameStateMachine gameStateMachine, IGameFactory gameFactory, IPortalSystem portalSystem, IInputService inputService,
            IDataProvider dataProvider)
        {
            _location = dataProvider.Locations[LocationId.TestScene];
            _portalSystem = portalSystem;
            _inputService = inputService;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameFactory.CreatePlayer(_location.PlayerSpawnPosition, _location.PlayerSpawnRotation);
            _inputService.SwitchInputScenario(InputScenario.Gameplay); 
            _gameStateMachine.SwitchState<WaveOngoingState>();
        }

        public void Exit()
        {
        }

      
    }
}