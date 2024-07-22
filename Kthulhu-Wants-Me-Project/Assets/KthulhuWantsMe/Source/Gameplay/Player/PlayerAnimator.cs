using System;
using KthulhuWantsMe.Source.Gameplay.AnimatorHelpers;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        public AnimatorState CurrentState { get; private set; }
        public Animator Animator => _animator;

        public Action<AnimatorState> OnStateEntered;
        public Action<AnimatorState> OnStateExited;

        [SerializeField] private Animator _animator;

        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackIndex = Animator.StringToHash("AttackIndex");
        private static readonly int SpecialAttack = Animator.StringToHash("SpecialAttack");
        private static readonly int Impact = Animator.StringToHash("Impact");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int LungeCharge = Animator.StringToHash("LungeCharge");
        private static readonly int Lunge = Animator.StringToHash("Lunge");
        public static readonly int Evade = Animator.StringToHash("Evade");

        
        private static readonly int _impactStateHash = Animator.StringToHash("Impact");
        private static readonly int _specialAttackStateHash = Animator.StringToHash("SpecialAttack");
        private static readonly int _evadeStateHash = Animator.StringToHash("Evade");

        private RuntimeAnimatorController _defaultAnimatorController;



        private void Start() =>
            _defaultAnimatorController = _animator.runtimeAnimatorController;

        public void Move()
        {
            _animator.SetBool(IsRunning, true);
        }

        public void StopMoving()
        {
            _animator.SetBool(IsRunning, false);
        }

        public void PlayLungeCharge()
        {
            _animator.SetBool(LungeCharge, true);
        }
        
        public void PlayEvade()
        {
            _animator.SetTrigger(Evade);
        }

        public void StopLungeCharge()
        {
            _animator.SetBool(LungeCharge, false);
        }

        public void PlayLunge()
        {
            StopLungeCharge();
            _animator.SetTrigger(Lunge);
        }

        public void ApplyWeaponMoveSet(WeaponMoveSet moveSet)
        {
            _animator.runtimeAnimatorController =
                moveSet == null ? _defaultAnimatorController : moveSet.MoveSetAnimations;
        }

        public void PlayAttack(int attackIndex)
        {
            _animator.SetTrigger(Attack);
            _animator.SetInteger(AttackIndex, attackIndex);
        }

        public void PlaySpecialAttack()
        {
            _animator.SetTrigger(SpecialAttack);
        }

        public void PlayImpact()
        {
            _animator.ResetTrigger(Attack);
            StopMoving();
            _animator.SetTrigger(Impact);
            _animator.SetBool(LungeCharge, false);
        }

        public void PlayDie()
        {
            _animator.SetTrigger(Die);
        }

        public void ResetAnimatorController() =>
            _animator.runtimeAnimatorController = _defaultAnimatorController;

        public void EnteredState(int stateHash)
        {
            CurrentState = StateFor(stateHash);
            OnStateEntered?.Invoke(CurrentState);
        }

        public void ExitedState(int stateHash)
        {
            AnimatorState exitedState = StateFor(stateHash);
            OnStateExited?.Invoke(exitedState);
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if (stateHash == _impactStateHash)
                state = AnimatorState.Impact;
            else if (stateHash == _specialAttackStateHash)
                state = AnimatorState.SpecialAttack;
            else if (stateHash == _evadeStateHash)
                state = AnimatorState.Evade;
            else
                state = AnimatorState.Unknown;
            return state;
        }
    }

    public enum AnimatorState
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Impact = 3,
        Die = 4,
        SpecialAttack = 5,
        Evade = 6,
        Unknown = 100,
    }
}