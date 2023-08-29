using System;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerTentacleInteraction : MonoBehaviour
    {
        public bool PlayerGrabbed { get; private set; }

        [SerializeField] private PlayerFacade _player;

        private Transform _target;
        private PlayerMovementController _movementController;
        private Action _onPlayerBrokeFree;

        private IInputService _inputService;

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

        public void FollowGrabTarget(Transform target, Action onPlayerBrokeFree)
        {
            _onPlayerBrokeFree = onPlayerBrokeFree;
            PlayerGrabbed = true;
            _movementController.KillVelocity();
            _movementController.ToggleMotor(false);
            _target = target;
        }

        private void StopFollowing()
        {
            _target = null;
            _movementController.ToggleMotor(true);
            PlayerGrabbed = false;
        }

        private void ResistGrab()
        {
            if(!PlayerGrabbed)
                return;

            StopFollowing();
            _onPlayerBrokeFree?.Invoke();
        }
    }
}