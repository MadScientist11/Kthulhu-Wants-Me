using KinematicCharacterController;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private KinematicCharacterMotor _kinematicCharacterMotor;
        private PlayerKinematicLocomotion _kinematicLocomotion;
        private PlayerConfiguration _playerConfiguration;
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, IDataProvider dataProvider)
        {
             _playerConfig = dataProvider.PlayerConfig;
             _inputService = inputService;
             _kinematicLocomotion = new PlayerKinematicLocomotion(_kinematicCharacterMotor, _playerConfig);
        }

        private void Update() => 
            ProcessInput();
        
        private void ProcessInput()
        {
            Vector3 vectorToHit = GetLookDirection();
            Vector2 movementInput = _inputService.GameplayScenario.MovementInput;

            _kinematicLocomotion.SetInputs(movementInput, vectorToHit.normalized);
            
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _kinematicLocomotion.AddVelocity(transform.forward * 100f);
            }
        }

        private Vector3 GetLookDirection()
        {
            Ray ray = MousePointer.GetWorldRay(Camera.main);
            Vector3 vectorToHit = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                vectorToHit = (hitPointXZ - transform.position);
            }

            return vectorToHit;
        }
    }
}