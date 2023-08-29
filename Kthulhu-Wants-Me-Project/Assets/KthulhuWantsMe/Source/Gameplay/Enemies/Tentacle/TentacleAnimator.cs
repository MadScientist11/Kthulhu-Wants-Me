using System;
using KthulhuWantsMe.Source.Gameplay.AnimatorHelpers;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAnimator : MonoBehaviour, IAnimationStateReader
    {
        private bool GrabPlayerAnimationActive
        {
            get => _grabPlayerAnimationActive;
            set
            {
                _tentacleMaterial.SetInt(GrabPlayer, Convert.ToInt32(value));
                _grabPlayerAnimationActive = value;
            }
        }
        
        public TentacleState CurrentState { get; private set; }

        public bool IsAttacking => CurrentState == TentacleState.Attack;

        public Action<TentacleState> OnStateEntered;
        public Action<TentacleState> OnStateExited;

        [SerializeField] private Renderer _tentacleRenderer;
        [SerializeField] private Animator _tentacleAnimator;
        [SerializeField] private Vector3 _tentacleGrabOffset;
        [SerializeField] private float _tentacleGrabRadius;
        [SerializeField] private float _twirlStrength;

        [SerializeField] private Transform _chainTarget;
        
        private Material _tentacleMaterial;
        private Transform _playerFollowTarget;
        private bool _grabPlayerAnimationActive;

        private static readonly int GrabPlayer = Shader.PropertyToID("_GrabPlayer");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int InteractPos = Shader.PropertyToID("_InteractPos");
        private static readonly int TwirlStrength = Shader.PropertyToID("_TwirlStrength");
        
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Aggro = Animator.StringToHash("Aggro");
        
        private static readonly int _idleStateHash = Animator.StringToHash("Idle");
        private static readonly int _attackStateHash = Animator.StringToHash("Attack");
        
        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Awake()
        {
            _tentacleMaterial = _tentacleRenderer.material;
        }

        private void Update()
        {
            if (GrabPlayerAnimationActive)
            {
                _tentacleMaterial.SetFloat(Radius, _tentacleGrabRadius);
                _tentacleMaterial.SetFloat(TwirlStrength, _twirlStrength);
                _tentacleMaterial.SetVector(InteractPos, _playerFollowTarget.localPosition + _tentacleGrabOffset);
            }
        }

        public void PlayGrabPlayerAnimation(Transform playerFollowTarget)
        {
            _playerFollowTarget = playerFollowTarget;
            GrabPlayerAnimationActive = true;
        }

        public void PlayIdle()
        {
            GrabPlayerAnimationActive = false;
        }

        public void PlayAttack()
        {
            _chainTarget.position = _player.transform.position;
            _tentacleAnimator.SetTrigger(Attack);
        }
        
        public void CancelAttack()
        {
            _tentacleAnimator.SetBool(Attack, false);
        }

        public void PlayAggroIdle()
        {
            _tentacleAnimator.SetBool(Aggro, true);
        }

        public void EnteredState(int stateHash)
        {
            CurrentState = StateFor(stateHash);
            OnStateEntered?.Invoke(CurrentState);
        }

        public void ExitedState(int stateHash) => 
            OnStateExited?.Invoke(CurrentState);

        private TentacleState StateFor(int stateHash)
        {
            TentacleState state;
            
            if (stateHash == _idleStateHash)
                state = TentacleState.Idle;
            else if (stateHash == _attackStateHash)
                state = TentacleState.Attack;
            else
                state = TentacleState.Unknown;
            
            return state;
        }
    }
}