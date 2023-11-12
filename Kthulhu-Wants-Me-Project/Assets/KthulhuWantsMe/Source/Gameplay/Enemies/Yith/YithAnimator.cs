using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int MoveHash = Animator.StringToHash("Move");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int CombatHash = Animator.StringToHash("Combat");
        private static readonly int StanceHash = Animator.StringToHash("Stance");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int ResetAttackHash = Animator.StringToHash("ResetAttack");

        public void PlayMove()
        {
            _animator.SetBool(MoveHash, true);
        }

        public void StopMove()
        {
            _animator.SetBool(MoveHash, false);
        }
        
        public void PlayStance(int stanceId)
        {
            _animator.SetBool(ResetAttackHash, false);
            _animator.SetInteger(StanceHash, stanceId);
            _animator.SetTrigger(CombatHash);
        }

        public void PlayAttack()
        {
            _animator.ResetTrigger(CombatHash);
            _animator.SetInteger(StanceHash, -1);
            _animator.SetTrigger(AttackHash);
        }
        
        public void ResetAttack()
        {
            _animator.SetBool(ResetAttackHash, true);
            _animator.SetInteger(StanceHash, -1);
            _animator.ResetTrigger(CombatHash);
            _animator.ResetTrigger(AttackHash);
        }

        public void PlayDie()
        {
            _animator.SetTrigger(DieHash);
        }
    }
}