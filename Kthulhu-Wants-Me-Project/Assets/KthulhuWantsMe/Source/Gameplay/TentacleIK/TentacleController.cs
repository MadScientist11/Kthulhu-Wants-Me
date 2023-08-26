using System;
using System.Collections;
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
            _tentacleFsm.AddState("Idle", new State(onEnter: state =>
            {
                Debug.Log("Idle");

                _tentacleAnimator.PlayIdleAnimation();
                _playerLocomotionController.SetFollowTarget(null);
            }));
            _tentacleFsm.AddState("GrabPlayer", new State(onEnter: state =>
            {
                Debug.Log("Grab");
                _tentacleAnimator.PlayGrabPlayerAnimation(_playerFollowTarget);
                _playerLocomotionController.SetFollowTarget(_playerFollowTarget);
            }));
            _tentacleFsm.AddState("Stunned", new CoState(this,onEnter: state =>
            {
                _tentacleAnimator.PlayIdleAnimation();
                _playerLocomotionController.SetFollowTarget(null);
                Debug.Log("Stunned");
            }, onLogic: Stun, needsExitTime:true));
            _tentacleFsm.SetStartState("Idle");

            _tentacleFsm.AddTransition(new Transition(
                "Idle",
                "GrabPlayer",
                (transition) => DistanceToPlayer() < 5f
            ));

            _tentacleFsm.AddTransition(new Transition(
                "GrabPlayer",
                "Stunned",
                (transition) => Input.GetKeyDown(KeyCode.Space)
            ));

            _tentacleFsm.AddTransition(new Transition(
                "Stunned",
                "Idle"
            ));
            _tentacleFsm.Init();
        }
        IEnumerator Stun(CoState<string, string> state)
        {
            while (state.timer.Elapsed < 3)
            {
                yield return null;
            }

            state.timer.Reset();
            state.fsm.StateCanExit();
        }

        private void Update()
        {
            _tentacleFsm.OnLogic();
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }

        private void SwitchState(TentacleState state)
        {
            if (_currentState == state)
                return;

            switch (state)
            {
                case TentacleState.Stunned:
                    StartCoroutine(TransitionToIdle(3f));
                    break;
                case TentacleState.Idle:
                    _tentacleAnimator.PlayIdleAnimation();
                    _playerLocomotionController.SetFollowTarget(null);
                    break;
                case TentacleState.GrabPlayer:
                    _tentacleAnimator.PlayGrabPlayerAnimation(_playerFollowTarget);
                    _playerLocomotionController.SetFollowTarget(_playerFollowTarget);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private IEnumerator TransitionToIdle(float after)
        {
            yield return new WaitForSeconds(after);
        }
    }
}