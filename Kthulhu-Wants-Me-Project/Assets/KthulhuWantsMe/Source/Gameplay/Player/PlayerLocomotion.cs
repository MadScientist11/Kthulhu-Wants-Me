using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public bool IsMoving => 
            _movementController.CurrentVelocity.XZ().sqrMagnitude > 0.1f && _motor.enabled;
        public PlayerMovementController MovementController => _movementController;

        [SerializeField] private KinematicCharacterMotor _motor;
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
            _movementController = new PlayerMovementController(_motor, _playerConfig);
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
        }

        private void ProcessInput()
        {
            Ray ray = MousePointer.GetWorldRay(UnityEngine.Camera.main);
            
            Vector3 vectorToHit = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                vectorToHit = (hitPointXZ - transform.position);
            }
            
            Vector2 movementInput = transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ()).XZ();
            _movementController.SetInputs(movementInput, vectorToHit.normalized);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _movementController.AddVelocity(transform.forward * 50f);
            }
        }
    }
}