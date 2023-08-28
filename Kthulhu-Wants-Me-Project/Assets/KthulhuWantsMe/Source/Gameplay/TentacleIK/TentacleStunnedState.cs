using System.Collections;
using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleStunnedState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly PlayerMovementController _movementController;
        private readonly TentacleController _tentacleController;

        public TentacleStunnedState(TentacleController tentacleController, TentacleAnimator tentacleAnimator, PlayerMovementController movementController) : base(needsExitTime: true)
        {
            _tentacleController = tentacleController;
            _movementController = movementController;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayIdleAnimation();
            _tentacleController.StartCoroutine(StunEffectWearOff(5f));
        }

        private IEnumerator StunEffectWearOff(float after)
        {
            yield return new WaitForSeconds(after);
            fsm.StateCanExit();
        }
    }
}