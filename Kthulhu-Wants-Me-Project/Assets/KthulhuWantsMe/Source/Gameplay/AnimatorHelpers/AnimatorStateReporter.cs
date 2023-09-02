using UnityEngine;
using UnityEngine.Animations;

namespace KthulhuWantsMe.Source.Gameplay.AnimatorHelpers
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimationStateReader _stateReader;

        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);
            FindReader(animator);

            _stateReader?.EnteredState(stateInfo.shortNameHash);
        }

        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindReader(animator);
            _stateReader?.ExitedState(stateInfo.shortNameHash);
            
        }

        private void FindReader(UnityEngine.Animator animator)
        {
            if(_stateReader != null)
                return;

            _stateReader = animator.gameObject.GetComponent<IAnimationStateReader>();
        }
    }
}