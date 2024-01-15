using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Utilities.Extensions;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class MinionSpawn : MonoBehaviour, ISpawnBehaviour
    {
        public EnemySpawnerId SpawnedAt { get; set; }
        
        public CancellationTokenSource SpawnToken { get; private set; }
        
        [SerializeField] private float _height;

        private Vector3 _desiredPosition;

        private IPortalFactory _portalFactory;
        private Portal _boundPortal;

        [Inject]
        public void Construct(IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
        }

        public void OnSpawn(Action onSpawned)
        {
            SpawnToken = new();
            SpawnToken.RegisterRaiseCancelOnDestroy(gameObject);
            SpawnToken.Token.Register(() =>
            {
                if(_boundPortal != null)
                    _boundPortal.ClosePortal();
                
                Destroy(gameObject);
            });
            SpawnVisual().Forget();
        }

        private async UniTaskVoid SpawnVisual()
        {
            IStoppable enemyLogic = GetComponent<IStoppable>();
            Collider enemyCollider = GetComponent<Collider>();
            _desiredPosition = transform.position;
            
            enemyLogic.StopEntityLogic();
            enemyCollider.enabled = false;
            
            WarpUnderground();
            _boundPortal = _portalFactory.GetOrCreatePortal(_desiredPosition, Quaternion.identity, EnemyType.Cyeagha);
            
            await UniTask.Delay(TimeSpan.FromSeconds(2), false, PlayerLoopTiming.Update, SpawnToken.Token);
            
            WarpToDesiredLocation();
            _boundPortal.ClosePortal();
            
            enemyLogic.ResumeEntityLogic();
            await UniTask.Delay(TimeSpan.FromSeconds(1), false, PlayerLoopTiming.Update, SpawnToken.Token);
            enemyCollider.enabled = true;
        }

        private void WarpUnderground()
        {
            transform.position = transform.position.AddY(-_height);
        }
        
        private void WarpToDesiredLocation()
        {
            transform.position = _desiredPosition;
        }
    }
}