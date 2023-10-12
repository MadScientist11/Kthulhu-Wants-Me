using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class MinionSpawn : MonoBehaviour, ISpawnBehaviour
    {
        public bool Spawned { get; set; } = true;
        public EnemySpawnerId SpawnedAt { get; set; }
        
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
            GetComponent<Collider>().enabled = false;
            GetComponent<IStoppable>().StopEntityLogic();
            _desiredPosition = transform.position;
            transform.position = transform.position.AddY(-_height);
            StartCoroutine(DoSpawnEnemy());
        }

        private IEnumerator DoSpawnEnemy()
        {
            Portal portal = _portalFactory.GetOrCreatePortal(_desiredPosition, Quaternion.identity, EnemyType.Cyeagha);
            yield return new WaitForSeconds(2f);
            transform.position = _desiredPosition;
            portal.ClosePortal();
            GetComponent<IStoppable>().ResumeEntityLogic();

            yield return new WaitForSeconds(1f);
            GetComponent<Collider>().enabled = true;
        }
    }
}