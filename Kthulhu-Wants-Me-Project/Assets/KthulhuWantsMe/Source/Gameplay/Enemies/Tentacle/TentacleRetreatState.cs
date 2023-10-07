using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleRetreatState : RetreatState
    {
        [SerializeField] private TentacleFacade _tentacleFacade;
        
        private ILootService _lootService;

        [Inject]
        public void Construct(ILootService lootService)
        {
            _lootService = lootService;
        }
        
        protected override void OnRetreat()
        {
            _tentacleFacade.TentacleAnimator.PlayRetreat();
            _tentacleFacade.BlockAIProcessing();
            _tentacleFacade.CancelActiveSpells();
        }

        protected override void OnRetreatDefeated()
        {
            _tentacleFacade.TentacleAnimator.PlayRetreat();
            _tentacleFacade.BlockAIProcessing();
            _tentacleFacade.CancelActiveSpells();
        }

        protected override void OnRetreated()
        {
           Destroy(gameObject);
        }

        protected override void OnRetreatedDefeated()
        {
            _lootService.SpawnRandomBuff(InitialPoint + Vector3.up, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}