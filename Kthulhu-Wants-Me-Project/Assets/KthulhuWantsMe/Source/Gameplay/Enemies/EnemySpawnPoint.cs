using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public EnemyType EnemyType;
        
        [SerializeField] private float _gizmoRadius;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _gizmoRadius);
        }
    }
}