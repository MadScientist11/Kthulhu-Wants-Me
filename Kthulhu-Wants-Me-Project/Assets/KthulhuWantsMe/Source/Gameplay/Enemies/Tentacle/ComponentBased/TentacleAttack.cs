using System.Linq;
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
        [SerializeField] private TentacleAggro _tentacleAggro;

        private float _attackCooldown;
        private bool _isAttacking;

        private TentacleConfiguration _tentacleConfig;


        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        private void Update()
        {
            UpdateAttackCooldown();

            if (_tentacleAggro.HasAggro && CanAttack())
            {
                _isAttacking = true;
                _tentacleAnimator.PlayAttack();
            }
        }

        public float ProvideDamage()
        {
            return 10;
        }

        private void OnAttack()
        {
            D.raw(new Shape.Sphere(AttackStartPoint(), _tentacleConfig.AttackRadius), 1f);

            if (!this.HitFirst(AttackStartPoint(), _tentacleConfig.AttackRadius, out IDamageable hitObject))
                return;

            hitObject.TakeDamage(ProvideDamage());
        }

        private void OnAttackEnd()
        {
            _attackCooldown = _tentacleConfig.AttackCooldown;
            _isAttacking = false;
        }

        private bool CanAttack()
        {
            return CooldownIsUp() && !_isAttacking;
        }

        private void UpdateAttackCooldown()
        {
            _attackCooldown -= Time.deltaTime;
        }

        private bool CooldownIsUp()
        {
            return _attackCooldown <= 0;
        }

        private bool Hit(out Collider col)
        {
            Collider[] results = new Collider[1];
            int hitObjectsCount =
                Physics.OverlapSphereNonAlloc(AttackStartPoint(), _tentacleConfig.AttackRadius, results);
            col = results.FirstOrDefault();
            return hitObjectsCount > 0;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}