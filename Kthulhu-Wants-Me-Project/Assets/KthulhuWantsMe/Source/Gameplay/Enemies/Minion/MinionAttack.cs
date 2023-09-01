using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAttack : MonoBehaviour, IDamageProvider
    {
        [SerializeField] private MinionAIBrain _minionAIBrain;
        [SerializeField] private MinionHealth _minionHealth;

        private float _attackCooldown;
        private const float AttackCooldownTime = 2f;

        private void Start()
        {
            _minionHealth.TookDamage += OnTookDamage;
        }

        private void OnDestroy()
        {
            _minionHealth.TookDamage -= OnTookDamage;
        }

        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown <= 0)
            {
                _minionAIBrain.AttackCooldownReached = true;
            }
        }

        public float ProvideDamage()
        {
            return 25;
        }

        public void PerformAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), 2, out IDamageable hitObject))
                return;

            ResetCooldown();
            hitObject.TakeDamage(ProvideDamage());
        }

        private void OnTookDamage() => 
            ResetCooldown();

        private void ResetCooldown()
        {
            _attackCooldown = AttackCooldownTime;
            _minionAIBrain.AttackCooldownReached = false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * .25f;
        }
    }
}