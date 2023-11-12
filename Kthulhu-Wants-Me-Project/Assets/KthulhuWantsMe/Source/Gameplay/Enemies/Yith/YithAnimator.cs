using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Combat = Animator.StringToHash("Combat");
        private static readonly int Stance = Animator.StringToHash("Stance");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void PlayMove()
        {
            _animator.SetBool(Move, true);
        }

        public void StopMove()
        {
            _animator.SetBool(Move, false);
        }
        
        public void PlayStance(int stanceId)
        {
            _animator.SetInteger(Stance, stanceId);
            _animator.SetTrigger(Combat);
        }

        public void PlayAttack()
        {
            _animator.SetTrigger(Attack);
        }

        public void PlayDie()
        {
            _animator.SetTrigger(Die);
        }
    }
}