using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using Unity.AI.Navigation;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ISceneDataProvider
    {
        AllSpawnPoints AllSpawnPoints { get; }
        Camera CameraController { get; }
        NavMeshSurface MapNavMesh { get; }
    }
    
    public class SceneDataProvider : MonoBehaviour, ISceneDataProvider
    {
        public AllSpawnPoints AllSpawnPoints => _allSpawnPoints;
        public Camera CameraController => _cameraController;

        public NavMeshSurface MapNavMesh => _mapNavMesh;

        [SerializeField] private AllSpawnPoints _allSpawnPoints;
        [SerializeField] private Camera _cameraController;
        [SerializeField] private NavMeshSurface _mapNavMesh;
    }
}
