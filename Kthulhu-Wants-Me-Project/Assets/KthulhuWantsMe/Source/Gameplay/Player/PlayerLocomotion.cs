using Cinemachine;
using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public bool IsMoving =>
            _movementController.CurrentVelocity.XZ().sqrMagnitude > 0.1f;
        
        [SerializeField] private KinematicCharacterMotor _kinematicCharacterMotor;
        [SerializeField] private PlayerAnimator _playerAnimator;
        
        private PlayerMovementController _movementController;
        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider)
        {
            _playerConfig = dataProvider.PlayerConfig;
            _inputService = inputService;
            _movementController = new PlayerMovementController(_kinematicCharacterMotor, _playerConfig);

           
        }

        private void Update()
        {

            if (IsMoving)
            {
                _playerAnimator.Move();

            }
            else
            {
                _playerAnimator.StopMoving();
            }
            
            if(!_playerAnimator.IsAttacking)
                ProcessInput();
            Debug.Log(_playerAnimator.CurrentState);
        }

        private void ProcessInput()
        {
            Vector3 lookDirection = new Vector3(_inputService.GameplayScenario.LookInput.x, 0,
                _inputService.GameplayScenario.LookInput.y);
            Vector2 movementInput = _inputService.GameplayScenario.MovementInput;


            _movementController.SetInputs(movementInput, UnityEngine.Camera.main.transform.rotation,
                _playerConfig.PlayerFPSCameraPrefab.name == "PlayerIsoCamera");

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _movementController.AddVelocity(transform.forward * 100f);
            }
        }
    }
}