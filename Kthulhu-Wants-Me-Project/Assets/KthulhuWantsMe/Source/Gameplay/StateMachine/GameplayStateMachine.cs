using KthulhuWantsMe.Source.Gameplay.StateMachine.States;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine
{
    public class GameplayStateMachine
    {
        public IGameplayState CurrentState
        {
            get
            {
                return _activeState;
            }
        }
        
        private IGameplayState _activeState;
        private readonly StatesFactory _statesFactory;

        public GameplayStateMachine(StatesFactory statesFactory)
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