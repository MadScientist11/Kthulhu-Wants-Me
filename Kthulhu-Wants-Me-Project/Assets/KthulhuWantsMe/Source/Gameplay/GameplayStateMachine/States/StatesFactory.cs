using System;
using System.Collections.Generic;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class StatesFactory
    {
        private readonly Dictionary<Type, IGameplayState> _gameplayStates = new();
        private readonly IObjectResolver _resolver;

        public StatesFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public IGameplayState GetOrCreate<TState>() where TState : IGameplayState
        {
            if(_gameplayStates.TryGetValue(typeof(TState), out IGameplayState gameplayState))
            {
                return gameplayState;
            }

            return _gameplayStates[typeof(TState)] = _resolver.Resolve<TState>();
        }

    }
}