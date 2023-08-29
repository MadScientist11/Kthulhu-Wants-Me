using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFollowTarget : MonoBehaviour
    {
        [SerializeField] private PlayerFacade _player;

        private PlayerMovementController _movementController;
        private Transform _target;

        private void Awake()
        {
            _movementController = _player.PlayerLocomotion.MovementController;
        }

        private void Update()
        {
            if (_target != null)
                transform.SetPositionAndRotation(_target.position, _target.rotation);
        }

        public void FollowTarget(Transform target)
        {
            _movementController.KillVelocity();
            _movementController.ToggleMotor(false);
            _target = target;
        }

        public void StopFollowing()
        {
            _target = null;
            _movementController.ToggleMotor(true);
        }
    }
}