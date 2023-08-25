using Cinemachine;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotionInputProcessor : MonoBehaviour
    {
        [SerializeField] private KinematicCharacterMotor _kinematicCharacterMotor;
        private PlayerKinematicLocomotion _kinematicLocomotion;
        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider)
        {
             _playerConfig = dataProvider.PlayerConfig;
             _inputService = inputService;
             _kinematicLocomotion = new PlayerKinematicLocomotion(_kinematicCharacterMotor, _playerConfig);
        }

        private void Update() => 
            ProcessInput();
        
        private void ProcessInput()
        {
            Vector3 lookDirection = new Vector3(_inputService.GameplayScenario.LookInput.x, 0, _inputService.GameplayScenario.LookInput.y);
            Vector2 movementInput = _inputService.GameplayScenario.MovementInput;

            _kinematicLocomotion.SetInputs(movementInput, UnityEngine.Camera.main.transform.rotation);
          
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _kinematicLocomotion.AddVelocity(transform.forward * 100f);
            }
        }

        
    }
}