using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public interface IRetreatBehaviour
    {
        void Retreat();
        void RetreatDefeated();
    }

    public class TentacleRetreat : MonoBehaviour, IRetreatBehaviour
    {
        [SerializeField] private TentacleFacade _tentacleFacade;
        
        [SerializeField] private float _retreatDuration;
        [SerializeField] private float _height;
        
        private Portal _boundPortal;
        private bool _spawnLootAfterRetreat;
        
        private ILootService _lootService;

        [Inject]
        public void Construct(ILootService lootService)
        {
            _lootService = lootService;
        }

        public void Init(Portal boundPortal)
        {
            _boundPortal = boundPortal;
        }
        
        public void Retreat()
        {
            StartCoroutine(RetreatToPortal(transform.position.AddY(-_height)));
        }

        public void RetreatDefeated()
        {
            _spawnLootAfterRetreat = true;
            Retreat();
        }

        private IEnumerator RetreatToPortal(Vector3 to)
        {
            OnRetreat();
            
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = to;

            float duration = _retreatDuration;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            OnRetreated();
        }

        private void OnRetreat()
        {
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
                BuffItem buffItem = _lootService.SpawnRandomBuff(transform.position + Vector3.up * _height * 1.2f, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
}