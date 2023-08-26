using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleGrabPlayerState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly PlayerLocomotionController _locomotionController;
        private readonly Transform _playerFollowTarget;

        public TentacleGrabPlayerState(TentacleAnimator tentacleAnimator, PlayerLocomotionController locomotionController, Transform playerFollowTarget) : base(needsExitTime: false)
        {
            _playerFollowTarget = playerFollowTarget;
            _locomotionController = locomotionController;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayGrabPlayerAnimation(_playerFollowTarget);
            _locomotionController.SetFollowTarget(_playerFollowTarget);
        }

        public override void OnExit()
        {
            _locomotionController.SetFollowTarget(null);
        }
    }
}