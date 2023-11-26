using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using QFSW.QC;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.InGameConsole
{
    public class InGameConsoleService : MonoBehaviour
    {
        private QuantumConsole _console;
        
        private IObjectResolver _resolver;
        private IUIFactory _uiFactory;
        private IInputService _inputService;

        [Inject]
        public void Construct(IObjectResolver resolver, IUIFactory uiFactory, IInputService inputService)
        {
            _inputService = inputService;
            _uiFactory = uiFactory;
            _resolver = resolver;
        }

        private  void Start()
        {
            Scene startUp = SceneManager.GetSceneByPath(GameConstants.Scenes.StartUpPath);
            SceneManager.MoveGameObjectToScene(gameObject, startUp);
            _console = _uiFactory.CreateConsoleUI();


            _inputService.GameScenario.ToggleConsole += ToggleConsole;
        }

        private void OnDestroy()
        {
            _inputService.GameScenario.ToggleConsole -= ToggleConsole;
        }

        private void ToggleConsole()
        {
            if (_console.IsActive)
            {
                _console.Deactivate();
            }
            else
            {
                _console.Activate();
            }
        }

        [Command("heal", MonoTargetType.All)]
        private void Heal(int amount)
        {
            ThePlayer player = _resolver.Resolve<ThePlayer>();
            player.Heal(amount);
        }

        
    }
}