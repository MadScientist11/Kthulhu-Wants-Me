using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Die = Animator.StringToHash("Die");

        public void PlayMove()
        {
            _animator.SetBool(Move, true);
        }

        public void PlayDie()
        {
            _animator.SetTrigger(Die);
        }

        public void StopMove()
        {
            _animator.SetBool(Move, false);
        }
    }
}