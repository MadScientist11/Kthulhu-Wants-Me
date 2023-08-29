using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.ComponentBased
{
    public class TentacleGrabAbility : MonoBehaviour, IDamageSource
    {
        public Transform DamageSourceObject => transform;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;
        [SerializeField] private Transform _grabTarget;
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }
        
        public void GrabPlayer()
        {
            if (!this.HitFirst(AttackStartPoint(), _tentacleConfig.AttackRadius, out Collider hitObject))
                return;

            if (hitObject.TryGetComponent(out PlayerTentacleInteraction playerTentacleInteraction))
            {
                _tentacleAIBrain.HoldsPlayer = true;
                _tentacleAnimator.PlayGrabPlayerAttack();
                playerTentacleInteraction.FollowGrabTarget(_grabTarget, OnPlayerBrokeFree);
            }
        }

        private void OnPlayerBrokeFree()
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