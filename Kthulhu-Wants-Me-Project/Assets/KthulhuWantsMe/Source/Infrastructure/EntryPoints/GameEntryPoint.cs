using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class GameEntryPoint : IInitializable
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameEntryPoint(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        
        }
        
        public void Initialize()
        {
            _gameStateMachine.SwitchState<StartGameState>();
           
        }
    }
}