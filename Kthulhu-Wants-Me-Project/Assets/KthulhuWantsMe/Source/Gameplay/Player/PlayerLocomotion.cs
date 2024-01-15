using Freya;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
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

        public Vector3 LastLookDirection => _lastLookDirection;
        public PlayerMovementController MovementController => _movementController;

        [SerializeField] private KinematicCharacterMotor _motor;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerLungeAbility _playerLungeAbility;


        private bool _blockMovement;
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
           // int mask = ~(LayerMasks.PlayerMask & LayerMasks.GroundMask);
           // bool b = DrawPhysics.SphereCast(transform.position, .2f,
           //     -GetMovementDirection(_inputService.GameplayScenario.MovementInput).XZtoXYZ(), out RaycastHit hit, 1f,
           //     ~LayerMasks.GroundMask);

            if (!MovementInputDetected())
            {
                _movementController.SetInputs(Vector2.zero, _lastLookDirection);
                _playerAnimator.StopMoving();
                return;
            }

            if (CanMove())
            {
                _playerAnimator.Move();
                //if(_playerAttack.InRecoveryPhase)
                //    _playerAttack.ResetAttackStateDelayed().Forget();
                
                _movementController.SetInputs(GetMoveDirection(), GetLookDirection());

            }
            else
            {
                _movementController.SetInputs(Vector2.zero, _lastLookDirection);
                _playerAnimator.StopMoving();
            }

        }

        public void StopToAttack()
        {
            _movementController.SetInputs(Vector2.zero, _lastLookDirection);
            _playerAnimator.StopMoving();
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
        
        public void BlockMovement(float timeFor)
        {
            _blockMovement = true;
            _movementController.ResetInputs();
            _coroutineRunner.ExecuteAfter(timeFor, () => _blockMovement = false);
        }

        private Vector2 GetMoveDirection()
        {
            //return GetMoveDirectionMouseBased().normalized;
            return GetMovementDirection(-_inputService.GameplayScenario.MovementInput);
        }

        private Vector3 GetLookDirection()
        {
            //return GetLookDirectionFromMouse();
            Vector2 moveDirection = GetMoveDirection();
            
            if (moveDirection.sqrMagnitude > Mathfs.Epsilon)
            {
                _lastLookDirection = moveDirection.XZtoXYZ();
            }
            return _lastLookDirection;
        }

        private Vector2 GetMoveDirectionMouseBased()
        {
            Vector3 lookDirection = GetLookDirectionFromMouse();
            Vector3 right = Vector3.Cross(transform.up, lookDirection);
            Vector2 movementInput = _inputService.GameplayScenario.MovementInput;
            return right.XZ() * movementInput.x + lookDirection.XZ() * movementInput.y;
        }

        private Vector3 GetLookDirectionFromMouse()
        {
            Ray ray = MousePointer.GetWorldRay(UnityEngine.Camera.main);

            Vector3 lookDirection = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                lookDirection = (hitPointXZ - transform.position).normalized;
            }

            return lookDirection;

        }
        
        public Vector2 GetMovementDirection(Vector2 movementInput)
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
            return (!_playerAttack.IsAttacking || _playerAttack.InRecoveryPhase) && !_blockMovement && !_playerLungeAbility.IsInLunge;
        }

        private bool MovementInputDetected()
        {
            return _inputService.GameplayScenario.MovementInput.sqrMagnitude > 0.1f;
        }
    }
}