using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleIdleState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;

        public TentacleIdleState(TentacleAnimator tentacleAnimator) : base(needsExitTime: false)
        {
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayIdle();
        }
    }
}