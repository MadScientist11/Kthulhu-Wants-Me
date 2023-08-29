using System.Collections;
using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleStunnedState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private TentacleFacade _tentacleFacade;

        public TentacleStunnedState(TentacleFacade tentacleFacade, TentacleAnimator tentacleAnimator) : base(needsExitTime: true)
        {
            _tentacleFacade = tentacleFacade;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayIdle();
            _tentacleFacade.StartCoroutine(StunEffectWearOff(5f));
        }

        private IEnumerator StunEffectWearOff(float after)
        {
            yield return new WaitForSeconds(after);
            fsm.StateCanExit();
        }
    }
}