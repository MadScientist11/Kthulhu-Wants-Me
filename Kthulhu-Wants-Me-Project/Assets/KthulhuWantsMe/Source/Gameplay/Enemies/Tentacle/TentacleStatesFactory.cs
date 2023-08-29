using System;
using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleStatesFactory
    {
        private TentacleFacade _tentacleFacade;
        private PlayerFacade _player;

        public TentacleStatesFactory(TentacleFacade tentacleFacade, PlayerFacade player)
        {
            _player = player;
            _tentacleFacade = tentacleFacade;
        }

        public StateBase Create(TentacleState state)
        {
            return state switch
            {
                TentacleState.Idle => new TentacleIdleState(_tentacleFacade.TentacleAnimator),
                TentacleState.GrabPlayer => new TentacleGrabPlayerState(_tentacleFacade.TentacleAnimator, _player.PlayerTentacleInteraction,
                    _tentacleFacade.GrabTarget),
                TentacleState.Stunned => new TentacleStunnedState(_tentacleFacade, _tentacleFacade.TentacleAnimator),
                TentacleState.Attack => new TentacleAttackState(_tentacleFacade.TentacleAnimator),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}