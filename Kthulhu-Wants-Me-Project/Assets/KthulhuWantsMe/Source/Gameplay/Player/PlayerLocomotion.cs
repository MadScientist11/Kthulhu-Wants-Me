using System;
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
        [SerializeField] private PlayerTentacleInteraction _playerTentacleInteraction;

        private bool _blockMovement;

        private PlayerMovementController _movementController;
        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _playerConfig = dataProvider.PlayerConfig;
            _inputService = inputService;
            _movementController = new PlayerMovementController(_motor, _playerConfig);
            inputService.GameplayScenario.Dash += PerformDash;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.Dash -= PerformDash;
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

            if (CanMove())
                ProcessInput();
        }

        public void BlockMovement(float timeFor)
        {
            _blockMovement = true;
            _movementController.ResetInputs();
            _coroutineRunner.ExecuteAfter(timeFor, () => _blockMovement = false);
        }

        private void ProcessInput()
        {
            Ray ray = MousePointer.GetWorldRay(UnityEngine.Camera.main);

            Vector3 lookDirection = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                lookDirection = (hitPointXZ - transform.position);
            }

            Vector2 movementInput = transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ())
                .XZ();
            _movementController.SetInputs(movementInput, lookDirection.normalized);
        }

        private void PerformDash()
        {
            //_locomotion.Motor.ForceUnground(0.1f);
            if(_playerTentacleInteraction.PlayerGrabbed)
                return;
            
            _movementController.AddVelocity(transform.forward * 50f);
        }

        private bool CanMove()
        {
            return !_playerAnimator.IsAttacking && !_blockMovement;
        }
        
    }
}