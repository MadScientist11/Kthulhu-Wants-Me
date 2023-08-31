using System.Collections;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class TentacleGrabAbilityResponse : MonoBehaviour, IAbilityResponse<TentacleGrabAbility>
    {
        public bool Grabbed { get; private set; }

        [SerializeField] private PlayerFacade _player;

        private Transform _target;
        private PlayerMovementController _movementController;

        private IInputService _inputService;
        private TentacleGrabAbility _ability;

        private Coroutine _tentacleDamageCoroutine;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.GameplayScenario.GrabResistence += ResistGrab;
        }

        private void Start() => 
            _movementController = _player.PlayerLocomotion.MovementController;

        private void OnDestroy()
        {
            _inputService.GameplayScenario.GrabResistence -= ResistGrab;
        }

        private void Update()
        {
            if (_target != null)
                transform.SetPositionAndRotation(_target.position, _target.rotation);
        }

        public void RespondTo(TentacleGrabAbility ability)
        {
            _ability = ability;
            FollowGrabTarget(ability.GrabTarget);
            _tentacleDamageCoroutine = StartCoroutine(DealDamageOnGrab(ability));
        }

        private IEnumerator DealDamageOnGrab(TentacleGrabAbility ability)
        {
            yield return new WaitForSeconds(ability.KillPlayerAfter);
            if (transform.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(ability.TentacleGrabDamage);
            }
            _ability.CancelGrab();
            StopFollowingGrabTarget();
        }

        private void ResistGrab()
        {
            if(!Grabbed)
                return;

            CancelDealDamageCoroutine();
            StopFollowingGrabTarget();
            _ability.CancelGrab();
        }

        private void FollowGrabTarget(Transform target)
        {
            Grabbed = true;
            _movementController.KillVelocity();
            _movementController.ToggleMotor(false);
            _target = target;
        }

        private void StopFollowingGrabTarget()
        {
            _target = null;
            _movementController.ToggleMotor(true);
            Grabbed = false;
        }

        private void CancelDealDamageCoroutine() => 
            StopCoroutine(_tentacleDamageCoroutine);
    }
}