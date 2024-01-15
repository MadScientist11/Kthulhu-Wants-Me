using System;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public interface IRetreatBehaviour
    {
        void Retreat(bool destroyObject, Action onRetreated = null);
        void RetreatDefeated();
    }

    public class TentacleRetreat : MonoBehaviour, IRetreatBehaviour
    {
        [SerializeField] private TentacleFacade _tentacleFacade;
        
        [SerializeField] private float _retreatDuration;
        [SerializeField] private float _height;
        
        private Portal _boundPortal;
        private bool _spawnLootAfterRetreat;
        private bool _destroyObject;
        
        private ILootService _lootService;
        private Action _onRetreated;

        [Inject]
        public void Construct(ILootService lootService)
        {
            _lootService = lootService;
        }

        public void Init(Portal boundPortal)
        {
            _boundPortal = boundPortal;
        }
        
        public void Retreat(bool destroyObject, Action onRetreated = null)
        {
            _onRetreated = onRetreated;
            _destroyObject = destroyObject;
            OnRetreat();
        }

        public void RetreatDefeated()
        {
            _spawnLootAfterRetreat = true;
            Retreat(true);
        }

        private void OnRetreat()
        {
            //_tentacleFacade.TentacleAnimator.EnableRootMotion();
            _tentacleFacade.TentacleAnimator.PlayRetreat();
            _tentacleFacade.BlockAIProcessing();
            _tentacleFacade.CancelActiveSpells();
        }

        private void OnRetreated()
        {
            if (_boundPortal != null)
                _boundPortal.ClosePortal();

            if (_spawnLootAfterRetreat)
            {
                BuffItem buffItem = _lootService.SpawnBuff<HealthSoul>(transform.position + Vector3.up * GameConstants.SpawnItemsElevation, Quaternion.identity);
            }

            _onRetreated?.Invoke();
            //_tentacleFacade.TentacleAnimator.DisableRootMotion();

            if(_destroyObject)
                Destroy(gameObject);
        }
    }
}