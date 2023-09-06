using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class HealthItem : MonoBehaviour
    {
        public float HealthReplenishAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerConsumeAbility consumeAbility))
            {
                consumeAbility.InstaConsume(this);
            }
        }
    }
}