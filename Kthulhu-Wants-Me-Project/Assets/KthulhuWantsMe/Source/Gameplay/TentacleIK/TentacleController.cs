using System;
using System.Collections;
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

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
            _playerLocomotionController = gameFactory.Player.PlayerLocomotionController;
        }

        private void Update()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 5f)
            {
                SwitchState(TentacleState.GrabPlayer);
            }
            else
            {
                SwitchState(TentacleState.Idle);
            }
        }

        private void SwitchState(TentacleState state)
        {
            if(_currentState == state)
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
            SwitchState(TentacleState.Idle);
        }
    }
}