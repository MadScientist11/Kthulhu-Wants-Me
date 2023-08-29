﻿using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleGrabPlayerState : StateBase
    {
        private readonly TentacleAnimator _tentacleAnimator;
        private readonly Transform _grabTarget;
        private readonly PlayerTentacleInteraction _playerTentacleInteraction;

        public TentacleGrabPlayerState(TentacleAnimator tentacleAnimator, PlayerTentacleInteraction playerTentacleInteraction, Transform grabTarget) : base(needsExitTime: false)
        {
            _playerTentacleInteraction = playerTentacleInteraction;
            _grabTarget = grabTarget;
            _tentacleAnimator = tentacleAnimator;
        }

        public override void OnEnter()
        {
            _tentacleAnimator.PlayGrabPlayerAnimation(_grabTarget);
            Debug.Log(_playerTentacleInteraction);
            Debug.Log(_grabTarget);
            //_playerTentacleInteraction.FollowGrabTarget(_grabTarget);
        }

        public override void OnExit()
        {
            //_playerTentacleInteraction.StopFollowing();
        }
    }
}