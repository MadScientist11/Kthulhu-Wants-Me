using System.Linq;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleAttack : MonoBehaviour, IDamageProvider, IDamageSource
    {
        public Transform DamageSourceObject => transform;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        public void PerformAttack()
        {
            _tentacleAIBrain.IsAttacking = true;
            _tentacleAnimator.PlayAttack();
        }

        public float ProvideDamage()
        {
            return 25;
        }

        private void OnAttack()
        {
            if (!this.HitFirst(AttackStartPoint(), _tentacleConfig.AttackRadius, out IDamageable hitObject))
                return;

            hitObject.TakeDamage(ProvideDamage());
        }

        private void OnAttackEnd()
        {
            _tentacleAIBrain.IsAttacking = false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}