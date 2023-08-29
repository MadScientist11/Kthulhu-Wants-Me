using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleImpact : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private TentacleAnimator _tentacleAnimator;

        private void Start()
        {
            _enemyHealth.OnHealthChanged += ReceiveDamageImpact;
        }

        private void OnDestroy()
        {
            _enemyHealth.OnHealthChanged -= ReceiveDamageImpact;
        }

        private void ReceiveDamageImpact(float damage)
        {
            _tentacleAnimator.PlayImpact();
        }
    }
}