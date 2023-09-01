using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleImpact : MonoBehaviour
    {
        [SerializeField] private TentacleHealth _tentacleHealth;
        [SerializeField] private TentacleAnimator _tentacleAnimator;

        private void Start()
        {
            _tentacleHealth.Changed += ReceiveDamageImpact;
        }

        private void OnDestroy()
        {
            _tentacleHealth.Changed -= ReceiveDamageImpact;
        }

        private void ReceiveDamageImpact(float damage)
        {
            _tentacleAnimator.PlayImpact();
        }
    }
}