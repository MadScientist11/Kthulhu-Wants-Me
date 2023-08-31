using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleGrabPlayerState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly Transform _grabTarget;
        private readonly TentacleGrabAbilityResponse _tentacleGrabAbilityResponse;

        public TentacleGrabPlayerState(TentacleAnimator tentacleAnimator, TentacleGrabAbilityResponse tentacleGrabAbilityResponse, Transform grabTarget) : base(needsExitTime: false)
        {
            _tentacleGrabAbilityResponse = tentacleGrabAbilityResponse;
            _grabTarget = grabTarget;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayGrabPlayerAnimation(_grabTarget);
            Debug.Log(_tentacleGrabAbilityResponse);
            Debug.Log(_grabTarget);
            //_playerTentacleInteraction.FollowGrabTarget(_grabTarget);
        }

        public override void OnExit()
        {
            //_playerTentacleInteraction.StopFollowing();
        }
    }
}