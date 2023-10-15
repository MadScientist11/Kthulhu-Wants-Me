using System;
using System.Collections;
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

        [FormerlySerializedAs("_enemy")] [SerializeField]
        private EnemyStatsContainer enemyStatsContainer;

        [SerializeField] private Collider _collider;
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        [SerializeField] private NavMeshMovement _movementMotor;
        [SerializeField] private AnimationCurve _knockbackCurve;

        private bool _doKnockback;

        private void Start()
        {
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

            //_movementMotor.enabled = false;
            StartCoroutine(DoKnockback());

            _hitFeedbacks?.PlayFeedbacks();
        }

        private IEnumerator DoKnockback()
        {
            _movementMotor.Velocity = -transform.forward * 5f;
            yield return new WaitForSeconds(.5f);
            //Vector3 jumpStartPos = transform.position;
            //Vector3 dest = transform.position - transform.forward * 1.5f;

            //for (float t = 0; t < 1; t += Time.deltaTime * 2f)
            //{
            //    transform.position = Vector3.Lerp(jumpStartPos, dest, _knockbackCurve.Evaluate(t));
            //    yield return null;
            //}
            //_movementMotor.enabled = true;
        }

        private void HandleDeath()
        {
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            _movementMotor.Velocity = -transform.forward * 15f;
            _collider.enabled = false;
            yield return new WaitForSeconds(.2f);
            GetComponent<IStoppable>().StopEntityLogic();
            Destroy(gameObject, 2f);
        }
    }
}