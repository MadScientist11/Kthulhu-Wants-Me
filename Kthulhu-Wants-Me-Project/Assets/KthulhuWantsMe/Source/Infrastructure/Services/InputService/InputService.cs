using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public interface IInputService : IInitializableService
    {
        GameplayScenario GameplayScenario { get; }
        UIScenario UIScenario { get; }
        GameScenario GameScenario { get; }
        IInputScenario ActiveScenario { get; }
        void SwitchInputScenario(InputScenario inputScenario);
    }

    public interface IInputScenario
    {
        void Enable();
        void Disable();
    }

    public class InputService : IInputService
    {
        public bool IsInitialized { get; set; }  
        private GameInput _input;
        private Dictionary<InputScenario, IInputScenario> _inputScenarios;

        private IInputScenario _activeInputScenario;

        public IInputScenario ActiveScenario => _activeInputScenario;

        public GameplayScenario GameplayScenario { get; private set; }
        public UIScenario UIScenario { get; private set; }
        public GameScenario GameScenario { get; private set; }
        
        
        public UniTask Initialize()
        {
            _input = new GameInput();
            GameplayScenario = new GameplayScenario(_input.Gameplay);
            UIScenario = new UIScenario(_input.UI);
            GameScenario = new GameScenario(_input.Game);

            _input.UI.SetCallbacks(UIScenario);
            _input.Gameplay.SetCallbacks(GameplayScenario);
            _input.Game.SetCallbacks(GameScenario);
            GameScenario.Enable();

            _inputScenarios = new()
            {
                { InputScenario.Gameplay, GameplayScenario },
                { InputScenario.UI, UIScenario },
            };
            return UniTask.CompletedTask;
        }

        public void SwitchInputScenario(InputScenario inputScenario)
        {
            if (inputScenario == InputScenario.None)
            {
                _activeInputScenario?.Disable();
                return;
            }
            _activeInputScenario?.Disable();
            _activeInputScenario = _inputScenarios[inputScenario];
            _activeInputScenario.Enable();
        }

      
    }
}