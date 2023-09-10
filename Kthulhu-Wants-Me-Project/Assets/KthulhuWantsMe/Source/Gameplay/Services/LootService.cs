using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface ILootService
    {
        BuffItem SpawnRandomBuff(Vector3 at, Quaternion rotation);
    }

    public class LootService : ILootService
    {
        private readonly IGameFactory _gameFactory;

        public LootService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        public BuffItem SpawnRandomBuff(Vector3 at, Quaternion rotation)
        {
            return _gameFactory.CreateHealItem(at, rotation);
        }
    }
}