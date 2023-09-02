﻿using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleGrabAbility : MonoBehaviour, IDamageSource, IAbility
    {
        public Transform DamageSourceObject => transform;
        public Transform GrabTarget;

        public float TentacleGrabDamage => _tentacleConfig.TentacleGrabDamage;
        public float KillPlayerAfter => 5f;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private TentacleConfiguration _tentacleConfig;


        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        public void GrabPlayer()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _tentacleConfig.AttackRadius,
                    out IAbilityResponse<TentacleGrabAbility> hitObject))
                return;

            Debug.Log("Grab Player");

            _tentacleAIBrain.HoldsPlayer = true;
            _tentacleAnimator.PlayGrabPlayerAttack();
            hitObject.RespondTo(this);
        }

        public void CancelGrab()
        {
            _tentacleAIBrain.HoldsPlayer = false;
            _tentacleAIBrain.Stunned = true;
            _tentacleAnimator.CancelGrab();
        }


        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}