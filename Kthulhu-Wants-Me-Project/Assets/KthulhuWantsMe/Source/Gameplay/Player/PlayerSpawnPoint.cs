using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        
        [SerializeField] private float _gizmoRadius;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, _gizmoRadius);
        }
    }
}
