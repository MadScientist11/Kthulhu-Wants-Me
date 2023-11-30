using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;

        [SerializeField] private Collider _collider;
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        [SerializeField] private MMFeedbacks _deathFeedback;

        [SerializeField] private CyaeghaFacade _cyaeghaFacade;
        [SerializeField] private MovementMotor _movementMotor;
        [SerializeField] private CyaeghaAnimator _cyaeghaAnimator;
        [SerializeField] private AnimationCurve _knockbackCurve;

        private CyaeghaConfiguration _cyaeghaConfiguration;
        
        private void Start()
        {
            _cyaeghaConfiguration = (CyaeghaConfiguration)enemyStatsContainer.Config;
            TookDamage += OnTookDamage;
            Died += HandleDeath;
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

            _movementMotor.AddVelocity(-transform.forward * _cyaeghaConfiguration.Knockback, _cyaeghaConfiguration.KnockbackTime);

            _hitFeedbacks?.PlayFeedbacks();
        }

        private void HandleDeath()
        {
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            _collider.enabled = false;
            _cyaeghaFacade.CyaeghaWiggle?.SwitchOff();
            _deathFeedback?.PlayFeedbacks();
            GetComponent<IStoppable>().StopEntityLogic();
            _cyaeghaAnimator.PlayDie();
            yield return new WaitForSeconds(.2f);
            Destroy(gameObject, 1f);
        }
    }
}