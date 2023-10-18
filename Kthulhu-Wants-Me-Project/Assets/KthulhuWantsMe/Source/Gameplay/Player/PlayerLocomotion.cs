using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotion : MonoBehaviour, IInterceptionCompliant
    {
        public bool IsMoving =>
            _movementController.CurrentVelocity.XZ().sqrMagnitude > 0.1f && _motor.enabled;

        public Vector3 AverageVelocity => MovementController.AverageVelocity;


        public PlayerMovementController MovementController => _movementController;

        [SerializeField] private KinematicCharacterMotor _motor;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAttack _playerAttack;

        private bool _blockMovement;
        private bool _blockInputs;
        private Vector3 _lastLookDirection;

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

        public void BlockInputAndResetPrevious(float timeFor)
        {
            BlockInput();
            _movementController.ResetInputs();
            _coroutineRunner.ExecuteAfter(timeFor, () => _blockInputs = false);
        }
        
        public void BlockInput()
        {
            _blockInputs = true;
        }
        
        public void AllowInput()
        {
            _blockInputs = false;
        }

        public Vector3 FaceMouse()
        {
            Ray ray = MousePointer.GetWorldRay(UnityEngine.Camera.main);

            _lastLookDirection = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            
            Vector3 desiredDirection = transform.forward;
            
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                desiredDirection = (hitPointXZ - transform.position).normalized;
                _lastLookDirection = desiredDirection;
            }

            return desiredDirection;
        }

        private void ProcessInput()
        {
            if(_blockInputs)
                return;
            //Vector2 movementInput = transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ()).XZ();
            //movementInput = transform.TransformDirection(_inputService.GameplayScenario.MovementInput.XZtoXYZ())

            Vector2 movementInput = -_inputService.GameplayScenario.MovementInput;
            movementInput = GetMovementDirection(movementInput);
            
            if (movementInput.sqrMagnitude > 0)
            {
                _lastLookDirection = movementInput.XZtoXYZ();
            }


            _movementController.SetInputs(movementInput, _lastLookDirection);
        }


        private Vector2 GetMovementDirection(Vector2 movementInput)
        {
            return (movementInput.x, movementInput.y) switch
            {
                (-1f, 0) => new Vector2(-0.71f, 0.71f).normalized,
                (0f, -1f) => new Vector2(-0.71f, -0.71f).normalized,
                (1f, 0) => new Vector2(0.71f, -0.71f).normalized,
                (0f, 1f) => new Vector2(0.71f, 0.71f).normalized,
                var (x, y) when x * y > 0 && x < 0 => new Vector2(-1f, 0f).normalized,
                var (x, y) when x * y > 0 && x > 0 => new Vector2(1f, 0f).normalized,
                (< 0, _) => new Vector2(0f, 1f).normalized,
                (> 0, _) => new Vector2(0f, -1f).normalized,
                _ => Vector2.zero
            };
        }


        private bool CanMove()
        {
            return !_playerAttack.IsAttacking && !_blockMovement;
        }

        private bool MovementInputDetected()
        {
            if (_blockInputs)
                return false;
            
            return _inputService.GameplayScenario.MovementInput.sqrMagnitude > 0.1f;
        }

    }
}