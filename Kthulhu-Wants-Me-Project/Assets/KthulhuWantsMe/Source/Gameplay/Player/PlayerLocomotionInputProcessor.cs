using Cinemachine;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotionInputProcessor : MonoBehaviour
    {
        private IInputService _inputService;
        private PlayerFacade _player;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IInputService inputService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _inputService = inputService;
        }

        private void Update()
        {
            ProcessInput();
            _gameFactory.Player.PlayerLocomotionController.UpdateState();
        }

        private void ProcessInput()
        {
            Vector3 lookDirection = new Vector3(_inputService.GameplayScenario.LookInput.x, 0,
                _inputService.GameplayScenario.LookInput.y);
            Vector2 movementInput = _inputService.GameplayScenario.MovementInput;

            _gameFactory.Player.PlayerLocomotionController.SetInputs(movementInput, UnityEngine.Camera.main.transform.rotation);


            if (Input.GetKeyDown(KeyCode.Q))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _gameFactory.Player.PlayerLocomotionController.AddVelocity(transform.forward * 100f);
            }
        }
    }
}