using System;
using System.Collections;
using Freya;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using Unity.AI.Navigation;
using UnityEngine;
using Vertx.Debugging;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface ILootService
    {
        BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem;
        BuffItem SpawnFlameSoul();
    }

    public class LootService : ILootService
    {
        private readonly IGameFactory _gameFactory;
        private readonly NavMeshSurface _navMeshSurface;

        private readonly RaycastHit[] _results = new RaycastHit[2];

        public LootService(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider)
        {
            _gameFactory = gameFactory;
            _navMeshSurface = sceneDataProvider.MapNavMesh;
        }

        public BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem
        {
            return _gameFactory.CreateBuffItem<T>(at, rotation);
        }

        public BuffItem SpawnFlameSoul()
        {
            PlayerFacade player = _gameFactory.Player;

            int iterations = 0;
            while (true)
            {
                Vector3 randomPosition = player.transform.position.AddY(4) + Random.insideUnitCircle.XZtoXYZ() * 50f;
                Bounds navMeshBounds = _navMeshSurface.navMeshData.sourceBounds;
                Bounds adjustedBounds = new Bounds(navMeshBounds.center, navMeshBounds.size - new Vector3(1,0,1) * 10);

                int count = DrawPhysics.SphereCastNonAlloc(randomPosition, .75f, Vector3.down, _results, 50);

                if (count == 1 && adjustedBounds.Contains(randomPosition) && IsGround(_results[0]))
                {
                    RaycastHit raycastHit = _results[0];
                    return SpawnBuff<FlameSoul>(raycastHit.point.AddY(1), Quaternion.identity);
                }

                iterations++;

                if (iterations > 100)
                    break;
            }


            throw new Exception("Couldn't spawn flame soul");
        }

        private bool IsGround(RaycastHit raycastHit)
        {
            return raycastHit.transform.gameObject.layer == LayerMask.NameToLayer(GameConstants.Layers.Ground);
        }
    }
}