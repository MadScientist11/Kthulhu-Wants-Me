using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class TryAgainWindow : BaseWindow
    {
        public override WindowId Id => WindowId.DefeatWindow;
        
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        public void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }

        private void Start()
        {
            _yesButton.onClick.AddListener(NewGame);
            _noButton.onClick.AddListener(Quit);
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveListener(NewGame);
            _noButton.onClick.RemoveListener(Quit);
        }

        private void NewGame()
        {
            _gameplayStateMachine.SwitchState<RestartGameState>();
        }

        private void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

    }
}