using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleIdleState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly PlayerMovementController _movementController;

        public TentacleIdleState(TentacleAnimator tentacleAnimator, PlayerMovementController movementController) : base(needsExitTime: false)
        {
            _movementController = movementController;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayIdleAnimation();
        }
    }
}