using UnityEngine.InputSystem;

namespace KthulhuWantsMe.Source.Infrastructure.Services.InputService
{
    public interface IInputService
    {
    }

    public class InputService : IInputService, GameInput.IInGameActions, GameInput.IUIActions
    {
        public void OnNewaction(InputAction.CallbackContext context)
        {
            
        }
    }
}