using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private Collider _collider;
        [SerializeField] private MovementMotor _movementMotor;
        [SerializeField] private MMFeedbacks _hitFeedbacks;

        private YithConfiguration _yithConfiguration;
        
        private void Start()
        {
            Revive();
            TookDamage += OnTookDamage;
            Died += HandleDeath;
            _yithConfiguration = (YithConfiguration)enemyStatsContainer.Config;
        }

        private void OnDestroy()
        {
            TookDamage -= OnTookDamage;
            Died -= HandleDeath;
        }

        private void OnTookDamage()
        {
            if (CurrentHealth <= 0)
                return;
            
            _movementMotor.AddVelocity(-transform.forward * _yithConfiguration.Knockback, _yithConfiguration.KnockbackTime);
            _hitFeedbacks?.PlayFeedbacks();
        }

        private void HandleDeath()
        {
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            //_movementMotor.Velocity = -transform.forward * 15f;
            _collider.enabled = false;
            yield return new WaitForSeconds(.2f);
            GetComponent<IStoppable>().StopEntityLogic();
            Destroy(gameObject, 2f);
        }
    }
}