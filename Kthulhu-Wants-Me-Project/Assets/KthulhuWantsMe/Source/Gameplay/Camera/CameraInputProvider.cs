using Cinemachine;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class CameraInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
    {
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public float GetAxisValue(int axis)
        {
            if (enabled)
            {
                Vector2 lookInput = _inputService.GameplayScenario.LookInput;

                switch (axis)
                {
                    case 0: return lookInput.x;
                    case 1: return lookInput.y;
                    case 2: return 0;
                }
            }

            return 0;
        }
    }
}