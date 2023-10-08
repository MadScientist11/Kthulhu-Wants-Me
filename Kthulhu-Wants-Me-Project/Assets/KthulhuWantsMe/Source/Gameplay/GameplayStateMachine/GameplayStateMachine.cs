using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine
{
    public class GameplayStateMachine
    {
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