using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int MoveHash = Animator.StringToHash("Move");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        public void PlayMove()
        {
            _animator.SetBool(MoveHash, true);
        }

        public void StopMove()
        {
            _animator.SetBool(MoveHash, false);
        }
        
        public void PlayAttack()
        {
            _animator.SetTrigger(AttackHash);
        }
        
        public void PlayDie()
        {
            _animator.SetTrigger(DieHash);
        }
    }
}