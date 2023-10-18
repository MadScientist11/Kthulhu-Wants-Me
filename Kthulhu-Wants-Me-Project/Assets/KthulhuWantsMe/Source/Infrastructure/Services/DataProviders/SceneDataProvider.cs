using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using Unity.AI.Navigation;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ISceneDataProvider
    {
        AllSpawnPoints AllSpawnPoints { get; }
        CameraController CameraController { get; }
        NavMeshSurface MapNavMesh { get; }
    }
    
    public class SceneDataProvider : MonoBehaviour, ISceneDataProvider
    {
        public AllSpawnPoints AllSpawnPoints => _allSpawnPoints;
        public CameraController CameraController => _cameraController;

        public NavMeshSurface MapNavMesh => _mapNavMesh;

        [SerializeField] private AllSpawnPoints _allSpawnPoints;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private NavMeshSurface _mapNavMesh;
    }
}
