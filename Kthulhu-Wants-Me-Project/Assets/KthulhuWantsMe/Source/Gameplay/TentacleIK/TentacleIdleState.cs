using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleIdleState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly PlayerLocomotionController _locomotionController;

        public TentacleIdleState(TentacleAnimator tentacleAnimator, PlayerLocomotionController locomotionController) : base(needsExitTime: false)
        {
            _locomotionController = locomotionController;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayIdleAnimation();
        }
    }
}