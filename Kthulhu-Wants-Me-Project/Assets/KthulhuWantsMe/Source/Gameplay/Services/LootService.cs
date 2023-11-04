using System;
using Freya;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
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

        private readonly RaycastHit[] _results = new RaycastHit[2];

        public LootService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem
        {
            return _gameFactory.CreateBuffItem<T>(at, rotation);
        }

        public BuffItem SpawnFlameSoul()
        {
            PlayerFacade player = _gameFactory.Player;

            bool spawned = false;
            int iterations = 0;
            while (!spawned || iterations > 100)
            {
                Vector3 randomPosition = player.transform.position.AddY(5) + Random.insideUnitCircle.XZtoXYZ() * 20f;
                int count = Physics.SphereCastNonAlloc(randomPosition, 1f, Vector3.down, _results, 50);

                if (count == 1 && IsGround(_results[0]))
                {
                    RaycastHit raycastHit = _results[0];
                    spawned = true;
                    return SpawnBuff<FlameSoul>(raycastHit.point, Quaternion.identity);
                }

                iterations++;
            }
          

            throw new Exception("Couldn't spawn flame soul");
        }

        private bool IsGround(RaycastHit raycastHit)
        {
            return raycastHit.transform.gameObject.layer == LayerMask.NameToLayer(GameConstants.Layers.Ground);
        }
    }
}