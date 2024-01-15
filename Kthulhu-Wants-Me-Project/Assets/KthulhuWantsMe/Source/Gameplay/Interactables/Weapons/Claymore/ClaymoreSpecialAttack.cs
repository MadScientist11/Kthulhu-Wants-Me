using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Infrastructure;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore
{

    public interface ISpecialAttack : IAbilityResponse<PlayerSpecialAttackAbility>
    {
    }
    public class ClaymoreSpecialAttack : MonoBehaviour, ISpecialAttack, IInjectable, IDamageProvider
    {
        [SerializeField] private ProjectileArc _projectileArcPrefab;
        
        private ProjectileArc _projectileArc;
        private float _disappearDelay;
        
        private ProjectileArcFactory _projectileArcFactory;

        [Inject]
        public void Construct(ProjectileArcFactory projectileArcFactory)
        {
            _projectileArcFactory = projectileArcFactory;
            _projectileArcFactory.Init(_projectileArcPrefab, this);
        }


        public void Apply(IDamageable to)
        {
            to.TakeDamage(ProvideDamage());
        }

        public Transform DamageDealer { get; }

        public float ProvideDamage()
        {
            return 20;
        }

        public void RespondTo(PlayerSpecialAttackAbility ability)
        {
            _projectileArc = _projectileArcFactory.GetOrCreateProjectile(ability.transform.position + ability.transform.forward
                + ability.transform.up,
                ability.transform.rotation);

        }
    }
}