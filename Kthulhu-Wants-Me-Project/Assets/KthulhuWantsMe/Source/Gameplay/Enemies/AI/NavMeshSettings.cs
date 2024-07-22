using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    public class NavMeshSettings : MonoBehaviour
    {
        private void Awake()
        {
            NavMesh.avoidancePredictionTime = 5f;
            NavMesh.pathfindingIterationsPerFrame = 5000;
        }
    }
}
