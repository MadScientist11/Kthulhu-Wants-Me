using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleGrabPlayerState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly PlayerMovementController _movementController;
        private readonly Transform _playerFollowTarget;

        public TentacleGrabPlayerState(TentacleAnimator tentacleAnimator, PlayerMovementController movementController, Transform playerFollowTarget) : base(needsExitTime: false)
        {
            _playerFollowTarget = playerFollowTarget;
            _movementController = movementController;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayGrabPlayerAnimation(_playerFollowTarget);
            //_movementController.SetFollowTarget(_playerFollowTarget);
        }

        public override void OnExit()
        {
           // _movementController.SetFollowTarget(null);
        }
    }
}