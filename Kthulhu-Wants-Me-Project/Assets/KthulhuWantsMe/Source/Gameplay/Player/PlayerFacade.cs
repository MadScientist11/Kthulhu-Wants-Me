using KthulhuWantsMe.Source.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }

        private void OnValidate()
        {
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
        }
    }
}