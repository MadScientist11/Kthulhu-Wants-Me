using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleGrabAbility : MonoBehaviour, IAbility
    {
        public bool HoldsPlayer { get; private set; }
        
        public Transform GrabTarget;

        public float TentacleGrabDamage => _tentacleGrabDamage;
        public float KillPlayerAfter => 5f;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        [SerializeField] private float _attackRadius;
        [SerializeField] private float _tentacleGrabDamage;
        [SerializeField] private float _attackEffectiveDistance;

        public void GrabPlayer()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _attackRadius,
                    LayerMasks.PlayerMask, out IAbilityResponse<TentacleGrabAbility> hitObject))
                return;


            HoldsPlayer = true;
            _tentacleAnimator.PlayGrabPlayerAttack();
            hitObject.RespondTo(this);
        }

        public void CancelGrab()
        {
            HoldsPlayer = false;
            _tentacleAIBrain.Stunned = true;
            _tentacleAnimator.CancelGrab();
        }


        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _attackEffectiveDistance;
        }
    }
}