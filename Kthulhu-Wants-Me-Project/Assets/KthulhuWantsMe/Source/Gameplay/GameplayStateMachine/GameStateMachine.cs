using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine
{
    public class GameStateMachine
    {
        private IGameplayState _activeState;
        private readonly StatesFactory _statesFactory;

        public GameStateMachine(StatesFactory statesFactory)
        {
            _statesFactory = statesFactory;
        }
        public void SwitchState<TState>() 
            where TState : IGameplayState
        {
            _activeState?.Exit();
            _activeState = _statesFactory.GetOrCreate<TState>();
            _activeState.Enter();
        }
    }

    public interface IGameplayState
    {
        void Enter();
        void Exit();
    }
}