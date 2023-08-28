using System;
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

        private static readonly int _idleStateHash = Animator.StringToHash("Idle");
        private static readonly int _runStateHash = Animator.StringToHash("Run");
        private static readonly int _attackStateHash = Animator.StringToHash("Attack");


        public void Move()
        {
            _animator.SetBool(IsRunning, true);
        }

        public void StopMoving()
        {
            _animator.SetBool(IsRunning, false);
        }

        public void PlayAttack()
        {
            _animator.SetTrigger(Attack);
        }

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
        Unknown = 100,
    }
}