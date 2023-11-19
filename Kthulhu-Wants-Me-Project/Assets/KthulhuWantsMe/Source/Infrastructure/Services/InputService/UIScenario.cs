namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public class UIScenario : GameInput.IUIActions, IInputScenario
    {
        private GameInput.UIActions _uiActions;

        public UIScenario(GameInput.UIActions uiActions)
        {
            _uiActions = uiActions;
        }

        public void Enable() =>
            _uiActions.Enable();

        public void Disable() =>
            _uiActions.Disable();
    }
}