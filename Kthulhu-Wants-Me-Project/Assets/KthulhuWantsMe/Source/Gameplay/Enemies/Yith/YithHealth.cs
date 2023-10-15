using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
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
        [SerializeField] private NavMeshMovement _movementMotor;
        [SerializeField] private MMFeedbacks _hitFeedbacks;
        
        private void Start()
        {
            Revive();
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
            
            StartCoroutine(DoKnockback());
            _hitFeedbacks?.PlayFeedbacks();
        }

        private void HandleDeath()
        {
            StartCoroutine(Death());
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