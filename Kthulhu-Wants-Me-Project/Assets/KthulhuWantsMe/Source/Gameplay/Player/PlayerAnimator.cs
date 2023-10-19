using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.AnimatorHelpers;
using KthulhuWantsMe.Source.Gameplay.Weapons;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        public AnimatorState CurrentState { get; private set; }

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

        
        private static readonly int _impactStateHash = Animator.StringToHash("Impact");
        private static readonly int Evade = Animator.StringToHash("Evade");

        private RuntimeAnimatorController _defaultAnimatorController;

        private bool _enableRootMotion;


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
            _enableRootMotion = true;
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

        public void ExitedState(int stateHash) =>
            OnStateExited?.Invoke(CurrentState);


        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if (stateHash == _impactStateHash)
                state = AnimatorState.Impact;
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
        Unknown = 100,
    }
}