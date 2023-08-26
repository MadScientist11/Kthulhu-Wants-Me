using Cinemachine;
using KinematicCharacterController;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        [field: SerializeField] public KinematicCharacterMotor PlayerMotor { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }
        public PlayerLocomotionController PlayerLocomotionController { get; set; }

        
    }
}