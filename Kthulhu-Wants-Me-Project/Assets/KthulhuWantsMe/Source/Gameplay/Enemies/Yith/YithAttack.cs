using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.Stats;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using Time = UnityEngine.Time;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAttack : Attack
    {
        protected override float BaseDamage => enemyStatsContainer.EnemyStats.Stats[StatType.BaseDamage];

        [FormerlySerializedAs("_enemy")] [SerializeField]
        private EnemyStatsContainer enemyStatsContainer;

        [SerializeField] private MMFeedbacks _attackFeedback;
        [SerializeField] private MMFeedbacks _attackPrepareFeedback;
        [SerializeField] private YithAnimator _yithAnimator;
        [SerializeField] private FollowPlayer _followPlayerBehaviour;

        private float _attackCooldown;
        private float _attackDelayTime;
        private bool _isAttacking;

        private CancellationTokenSource _cancelAttackToken;

        private YithConfiguration _yithConfiguration;

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)enemyStatsContainer.Config;
        }

        private void Update() =>
            UpdateCountdowns();

        public async UniTaskVoid PerformAttack()
        {
            _cancelAttackToken = new();
            _cancelAttackToken.RegisterRaiseCancelOnDestroy(gameObject);
            
            _isAttacking = true;
            _yithAnimator.PlayStance(2);
            _attackPrepareFeedback?.PlayFeedbacks();

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), false, PlayerLoopTiming.Update, _cancelAttackToken.Token);

            _yithAnimator.PlayAttack();

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), false, PlayerLoopTiming.Update, _cancelAttackToken.Token);

            if (!PhysicsUtility.HitFirst(
                    transform,
                    AttackStartPoint(),
                    _yithConfiguration.AttackRadius,
                    LayerMasks.PlayerMask,
                    out IDamageable damageable))
            {
                ResetAttackState();
                return;
            }


            ApplyDamage(to: damageable);
            _attackFeedback?.PlayFeedbacks();

            ResetAttackState();
        }

        public void ResetAttackState()
        {
            _cancelAttackToken?.Cancel();
            _isAttacking = false;
            ResetCountdowns();
        }

        private void UpdateCountdowns()
        {
            _attackCooldown -= Time.deltaTime;

            if (_followPlayerBehaviour.PlayerReached)
                _attackDelayTime -= Time.deltaTime;
            else
                ResetAttackDelayCountdown();
        }

        private void ResetCountdowns()
        {
            _attackCooldown = _yithConfiguration.AttackCooldown;
            ResetAttackDelayCountdown();
        }

        private void ResetAttackDelayCountdown() =>
            _attackDelayTime = _yithConfiguration.AttackDelay;

        private bool CountdownsAreUp() =>
            _attackDelayTime <= 0 && _attackCooldown <= 0;

        public bool CanAttack()
        {
            return !_isAttacking && CountdownsAreUp();
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}