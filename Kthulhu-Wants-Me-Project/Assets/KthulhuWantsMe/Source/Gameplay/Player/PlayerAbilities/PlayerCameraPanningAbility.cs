using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerCameraPanningAbility : MonoBehaviour, IAbility
    {
        private PlayerFacade _player;
        
        private IGameFactory _gameFactory;
        private CameraController _cameraController;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _player = _gameFactory.Player;
        }
    }
}