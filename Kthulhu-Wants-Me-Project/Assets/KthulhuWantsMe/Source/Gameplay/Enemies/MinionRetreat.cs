using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class MinionRetreat : MonoBehaviour, IRetreatBehaviour
    {
        private IPortalFactory _portalFactory;

        [Inject]
        public void Construct(IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
        }
        
        public void Retreat()
        {
            StartCoroutine(DoRetreat());
        }

        public void RetreatDefeated()
        {
        }
        
        private IEnumerator DoRetreat()
        {
            GetComponent<IStoppable>().StopEntityLogic();
            Portal portal = _portalFactory.GetOrCreatePortal(transform.position, Quaternion.identity, EnemyType.Tentacle);
            yield return new WaitForSeconds(2f);
            portal.ClosePortal();
            Destroy(gameObject);
        }
        
    }
}