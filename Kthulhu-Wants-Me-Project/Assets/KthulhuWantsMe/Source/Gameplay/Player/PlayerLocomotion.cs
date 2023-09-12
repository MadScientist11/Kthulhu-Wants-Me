using System;
using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public enum Input
    {
        AscentLike = 0,
        RelativeToMouse = 1,
    }
    public class PlayerLocomotion : MonoBehaviour
    {
        public bool IsMoving =>
            _movementController.CurrentVelocity.XZ().sqrMagnitude > 0.1f && _motor.enabled;
        
        public PlayerMovementController MovementController => _movementController;

        [SerializeField] private KinematicCharacterMotor _motor;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAttack _playerAttack;

        private bool _blockMovement;

        private PlayerMovementController _movementController;
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
        }

        private void Update()
        {
            if (CanMove())
            {
                if (MovementInputDetected())
                    _playerAnimator.Move();
                else
                    _playerAnimator.StopMoving();
                
                ProcessInput();
                return;
            }

            _movementController.SetInputs(Vector2.zero, _lastLookDirection);
            _playerAnimator.StopMoving();
        }

        public void BlockMovement(float timeFor)
        {
            _blockMovement = true;
            _movementController.ResetInputs();
            _coroutineRunner.ExecuteAfter(timeFor, () => _blockMovement = false);
        }

        private Vector3 _lastLookDirection;

        private void ProcessInput()
        {
            Ray ray = MousePointer.GetWorldRay(UnityEngine.Camera.main);

            _lastLookDirection = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                _lastLookDirection = (hitPointXZ - transform.position).normalized;
            }

            //Vector2 movementInput = transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ()).XZ();

            Vector2 movementInput = _playerConfig.InputType switch
            {
                Input.AscentLike => -_inputService.GameplayScenario.MovementInput.YX(),
                Input.RelativeToMouse => transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ()).XZ(),
                _ => throw new ArgumentOutOfRangeException()
            };
            _movementController.SetInputs(movementInput, _lastLookDirection);
        }

        private bool CanMove()
        {
            return !_playerAttack.IsAttacking && !_blockMovement;
        }

        private bool MovementInputDetected()
        {
            return _inputService.GameplayScenario.MovementInput.sqrMagnitude > 0.1f;
        }
    }
}