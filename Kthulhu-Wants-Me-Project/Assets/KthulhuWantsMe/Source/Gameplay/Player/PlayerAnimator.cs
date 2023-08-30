using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.AnimatorHelpers;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        public bool IsAttacking => CurrentState == AnimatorState.Attack;
        public AnimatorState CurrentState { get; private set; }

        public Action<AnimatorState> OnStateEntered;
        public Action<AnimatorState> OnStateExited;

        [SerializeField] private Animator _animator;

        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Impact = Animator.StringToHash("Impact");
        private static readonly int Die = Animator.StringToHash("Die");

        private static readonly int _idleStateHash = Animator.StringToHash("Idle");
        private static readonly int _runStateHash = Animator.StringToHash("Run");
        private static readonly int _attackStateHash = Animator.StringToHash("Attack");
        private static readonly int _dieStateHash = Animator.StringToHash("Die");
        private static readonly int _impactStateHash = Animator.StringToHash("Impact");
        
        private RuntimeAnimatorController _defaultAnimatorController;


        private void Start()
        {
            _defaultAnimatorController = _animator.runtimeAnimatorController;
        }

   
        public void Move()
        {
            _animator.SetBool(IsRunning, true);
        }

        public void StopMoving()
        {
            _animator.SetBool(IsRunning, false);
        }

        public void PlayAttack(AnimatorOverrideController attackOverrideController)
        {
            _animator.runtimeAnimatorController = attackOverrideController == null ? _defaultAnimatorController : attackOverrideController;
            _animator.SetTrigger(Attack);
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
            
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _runStateHash)
                state = AnimatorState.Run;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack; 
            else if (stateHash == _impactStateHash)
                state = AnimatorState.Impact;
            else if (stateHash == _dieStateHash)
                state = AnimatorState.Die;
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