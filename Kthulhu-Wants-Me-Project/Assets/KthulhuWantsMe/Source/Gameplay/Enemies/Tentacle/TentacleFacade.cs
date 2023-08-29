using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleFacade : MonoBehaviour
    {
        [field: SerializeField] public TentacleAnimator TentacleAnimator { get; private set; }
        [field: SerializeField] public Transform GrabTarget { get; private set; }
    }
}