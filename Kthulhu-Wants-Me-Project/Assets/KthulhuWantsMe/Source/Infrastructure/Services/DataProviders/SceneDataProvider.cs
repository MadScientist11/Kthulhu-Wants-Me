using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ISceneDataProvider
    {
        AllSpawnPoints AllSpawnPoints { get; }
        CameraController CameraController { get; }
    }
    
    public class SceneDataProvider : MonoBehaviour, ISceneDataProvider
    {
        public AllSpawnPoints AllSpawnPoints => _allSpawnPoints;
        public CameraController CameraController => _cameraController;

        [SerializeField] private AllSpawnPoints _allSpawnPoints;
        [SerializeField] private CameraController _cameraController;
    }
}
