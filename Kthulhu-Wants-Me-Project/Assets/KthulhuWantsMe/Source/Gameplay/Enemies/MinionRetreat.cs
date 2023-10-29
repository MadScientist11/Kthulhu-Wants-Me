using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
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
        
        public void Retreat(bool destroyObject, Action onRetreated = null)
        {
            RetreatVisual().Forget();
        }

        public void RetreatDefeated()
        {
        }

        private async UniTaskVoid RetreatVisual()
        {
            MinionSpawn minionSpawn = GetComponent<MinionSpawn>();
            IStoppable enemyLogic = GetComponent<IStoppable>();
            Collider enemyCollider = GetComponent<Collider>();
            
            minionSpawn.SpawnToken.Cancel();
            enemyLogic.StopEntityLogic();
            enemyCollider.enabled = false;
            
            Portal portal = _portalFactory.GetOrCreatePortal(transform.position, Quaternion.identity, EnemyType.Cyeagha);
            
            await UniTask.Delay(TimeSpan.FromSeconds(2), false, PlayerLoopTiming.Update, destroyCancellationToken);
            
            portal.ClosePortal();
            Destroy(gameObject);
        }
    }
}