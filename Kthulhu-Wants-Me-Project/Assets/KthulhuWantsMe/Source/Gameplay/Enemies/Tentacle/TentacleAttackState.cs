using System.Collections;
using FSM;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAttackState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;

        public TentacleAttackState(TentacleAnimator tentacleAnimator) : base(true)
        {
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayAttack();
            _tentacleAnimator.StartCoroutine(AttackWearOff(0.5f));
        }
        

        public override void OnExit()
        {
            _tentacleAnimator.CancelAttack();
        }
        
        private IEnumerator AttackWearOff(float after)
        {
            yield return new WaitForSeconds(after);
            fsm.StateCanExit();
        }
    }
}