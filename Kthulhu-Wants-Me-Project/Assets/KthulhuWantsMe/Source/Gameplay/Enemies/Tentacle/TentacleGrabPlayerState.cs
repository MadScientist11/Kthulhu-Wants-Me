using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleGrabPlayerState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly Transform _grabTarget;
        private readonly PlayerFollowTarget _playerFollowTarget;

        public TentacleGrabPlayerState(TentacleAnimator tentacleAnimator, PlayerFollowTarget playerFollowTarget, Transform grabTarget) : base(needsExitTime: false)
        {
            _playerFollowTarget = playerFollowTarget;
            _grabTarget = grabTarget;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayGrabPlayerAnimation(_grabTarget);
            Debug.Log(_playerFollowTarget);
            Debug.Log(_grabTarget);
            _playerFollowTarget.FollowTarget(_grabTarget);
        }

        public override void OnExit()
        {
            _playerFollowTarget.StopFollowing();
        }
    }
}