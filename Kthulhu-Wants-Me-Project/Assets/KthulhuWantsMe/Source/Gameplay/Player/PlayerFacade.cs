using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }
        [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
        [field: SerializeField] public TentacleGrabAbilityResponse TentacleGrabAbilityResponse { get; private set; }
        [field: SerializeField]  public PlayerInteractionAbility PlayerInteractionAbility { get; private set; }
        [field: SerializeField]  public TentacleSpellResponse TentacleSpellResponse { get; private set;  }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        public CinemachineCameraPanning GetCameraPanningLogic() => 
            PlayerVirtualCamera.GetComponent<CinemachineCameraPanning>();


        public void ChangePlayerLayer(int layer)
        {
            gameObject.layer = layer;
            PlayerLocomotion.MovementController.RebuildMovementCollisionsLayerMask();
        }
    }
}