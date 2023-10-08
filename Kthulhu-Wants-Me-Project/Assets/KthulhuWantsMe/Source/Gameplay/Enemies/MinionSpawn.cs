﻿using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class MinionSpawn : MonoBehaviour, ISpawnBehaviour
    {
        [SerializeField] private float _height;

        private Vector3 _desiredPosition;
        
        private IPortalFactory _portalFactory;

        [Inject]
        public void Construct(IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
        }
        
        public void OnSpawn()
        {
            GetComponent<IStoppable>().StopEntityLogic();
            _desiredPosition = transform.position;
            transform.position = transform.position.AddY(-_height);
            Debug.Log(transform.position);
            StartCoroutine(DoSpawnEnemy());
        }

        private IEnumerator DoSpawnEnemy()
        {
            Portal portal = _portalFactory.GetOrCreatePortal(_desiredPosition, Quaternion.identity, EnemyType.Tentacle);
            yield return new WaitForSeconds(4f);
            transform.position = _desiredPosition;
            portal.ClosePortal();
            GetComponent<IStoppable>().ResumeEntityLogic();
        }
    }
}