using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.SpawnSystem
{
    public class SpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        public float Radius => _gizmoSize;

        
        public SpawnPointType SpawnPointType;

        [ShowIf("SpawnPointType", SpawnPointType.EnemySpawner)]
        public EnemySpawnerId EnemySpawnerId;
        
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gizmoSize;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(transform.position, _gizmoSize);
        }
    }
}
