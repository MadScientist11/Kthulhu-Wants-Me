using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Spell
{
    public class MinionsSpawnSpell : MonoBehaviour
    {
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Activate()
        {
            
        }
        
        public void Deactivate()
        {
            
        }
        
        private void BatchSpawn(int count)
        {
            
        }
    }
}