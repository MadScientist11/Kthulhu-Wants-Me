using Cinemachine;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        private void OnValidate()
        {
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
        }
    }
}