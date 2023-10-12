﻿using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public interface ISpawnBehaviour
    {
        EnemySpawnerId SpawnedAt { get; set; }
        void OnSpawn(Action onSpawned = null);
    }

    public class TentacleEmerge : MonoBehaviour, ISpawnBehaviour
    {
        public bool Spawned { get; set; }
        public EnemySpawnerId SpawnedAt { get; set; }

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleFacade _tentacleFacade;

        [SerializeField] private float _emergeDuration;
        [SerializeField] private float _height;

        private IPortalFactory _portalFactory;
        private Action _onSpawned;


        [Inject]
        public void Construct(IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
        }


        public void OnSpawn(Action onSpawned)
        {
            _onSpawned = onSpawned;
            GetComponent<Collider>().enabled = false;
            Portal portal = _portalFactory.GetOrCreatePortal(transform.position, Quaternion.identity, EnemyType.Tentacle);
            GetComponent<TentacleRetreat>().Init(portal);
            StartCoroutine(EmergeFromPortal(transform.position.AddY(-_height), transform.position));
        }
        
        private IEnumerator EmergeFromPortal(Vector3 from, Vector3 to)
        {
            OnEmerge();
            
            Vector3 initialPosition = from;
            Vector3 targetPosition = to;

            float duration = _emergeDuration;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            
            OnEmerged();
            Spawned = true;
            
            yield return new WaitForSeconds(1f);
            GetComponent<Collider>().enabled = true;
        }

        private void OnEmerge()
        {
            _tentacleFacade.ResetState();
            _tentacleFacade.BlockAIProcessing();
            _tentacleAnimator.PlayEmerge();
        }

        private void OnEmerged()
        {
            _tentacleAnimator.SetEmerged();
            _tentacleFacade.ResumeAIProcessing();
            _onSpawned?.Invoke();
        }
    }
}