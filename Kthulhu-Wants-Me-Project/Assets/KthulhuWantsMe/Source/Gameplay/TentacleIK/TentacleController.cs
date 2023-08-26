using System;
using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public enum TentacleState
    {
        Idle = 0,
        GrabPlayer = 1,
        Stunned = 2,
    }


    public class TentacleController : MonoBehaviour
    {
        [SerializeField] private Transform _playerFollowTarget;
        [SerializeField] private TentacleAnimator _tentacleAnimator;

        private PlayerFacade _player;
        private PlayerLocomotionController _playerLocomotionController;

        private TentacleState _currentState;

        private StateMachine _tentacleFsm;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
            _playerLocomotionController = gameFactory.Player.PlayerLocomotionController;
        }

        private void Start()
        {
            _tentacleFsm = new StateMachine();
            _tentacleFsm.AddState(TentacleState.Idle.ToString(), new TentacleIdleState(_tentacleAnimator, _playerLocomotionController));
            _tentacleFsm.AddState(TentacleState.GrabPlayer.ToString(), new TentacleGrabPlayerState(_tentacleAnimator, _playerLocomotionController, _playerFollowTarget));
            _tentacleFsm.AddState(TentacleState.Stunned.ToString(), new TentacleStunnedState(this, _tentacleAnimator, _playerLocomotionController));
            
            _tentacleFsm.SetStartState(TentacleState.Idle.ToString());

            _tentacleFsm.AddTransition(new Transition(
                TentacleState.Idle.ToString(),
                TentacleState.GrabPlayer.ToString(),
                (transition) => DistanceToPlayer() < 5f
            ));

            _tentacleFsm.AddTransition(new Transition(
                TentacleState.GrabPlayer.ToString(),
                TentacleState.Stunned.ToString(),
                (transition) => Input.GetKeyDown(KeyCode.Space)
            ));

            _tentacleFsm.AddTransition(new Transition(
                TentacleState.Stunned.ToString(),
                TentacleState.Idle.ToString()
            ));
            _tentacleFsm.Init();
        }

        private void Update()
        {
            Debug.Log(_tentacleFsm.ActiveStateName);
            _tentacleFsm.OnLogic();
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
    }
}