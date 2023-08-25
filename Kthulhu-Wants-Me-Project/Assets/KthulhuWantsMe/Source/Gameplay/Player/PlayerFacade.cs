using Cinemachine;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotionInputProcessor PlayerLocomotionInputProcessor { get; private set; }
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        private void OnValidate()
        {
            PlayerLocomotionInputProcessor = GetComponent<PlayerLocomotionInputProcessor>();
        }
    }
}