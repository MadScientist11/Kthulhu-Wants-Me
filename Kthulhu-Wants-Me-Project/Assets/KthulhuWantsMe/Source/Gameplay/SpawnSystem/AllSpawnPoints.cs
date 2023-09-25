using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SpawnSystem
{
    public class AllSpawnPoints : MonoBehaviour
    {
        [SerializeField] private List<SpawnPoint> _spawnPoints;

        public IEnumerable<SpawnPoint> this[SpawnPointType spawnPointType] 
            => _spawnPoints.Where(sp => sp.SpawnPointType == spawnPointType);

        private void OnValidate()
        {
            _spawnPoints.Clear();
            
            foreach (SpawnPoint spawnPoint in GetComponentsInChildren<SpawnPoint>())
            {
                _spawnPoints.Add(spawnPoint);
            }
        }
    }
}
