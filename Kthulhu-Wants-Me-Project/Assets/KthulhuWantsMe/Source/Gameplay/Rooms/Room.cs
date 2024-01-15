using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Freya;
using KthulhuWantsMe.Source.Infrastructure;
using Vertx.Debugging;
using Random = Freya.Random;

namespace KthulhuWantsMe.Source.Gameplay.Rooms
{
    public interface IRoom
    {
        Transform Transform { get; }
        bool Locked { get; }
        bool Contains(Vector3 point);
        Vector3 GetRandomPositionInside();
    }
    public class Room : MonoBehaviour, IRoom, IInjectable
    {
        public Transform Transform => transform;

        public bool Locked
        {
            get
            {
                if (_roomBarriers.Count == 0)
                {
                    return false;
                }
                
                return _roomBarriers.All(barrier => !barrier.Unlocked);
            }
        }

        [SerializeField] private List<FogWall> _roomBarriers;
        [SerializeField] private List<Collider> _roomColliders;

        private IRoomOverseer _roomOverseer;

        [Inject]
        public void Construct(IRoomOverseer roomOverseer)
        {
            _roomOverseer = roomOverseer;
        }

        private void Start() => 
            _roomOverseer.Register(this);

        private void OnDestroy() => 
            _roomOverseer.Unregister(this);

        public bool Contains(Vector3 point)
        {
            foreach (Collider roomCollider in _roomColliders)
            {
                if (roomCollider.bounds.Contains(point))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
           // Gizmos.DrawSphere(GetRandomPositionInside(), 1f);
        }

        public Vector3 GetRandomPositionInside()
        {
            Collider roomPartCollider = _roomColliders[Random.Range(0, _roomColliders.Count)];
            Vector3 randomPositionInside = Vector3.positiveInfinity;

            int iterations = 0;

            while (!ValidRandomPosition(in randomPositionInside, roomPartCollider))
            {
                Vector3 inUnitSphere = Random.InUnitSphere;
                Vector3 hemiSphereDirections = new Vector3(inUnitSphere.x, -Mathfs.Abs(inUnitSphere.y), inUnitSphere.z);
                float raycastOriginElevation = 5f;
                Ray ray = new(roomPartCollider.bounds.center.AddY(raycastOriginElevation), hemiSphereDirections);

                RaycastHit[] results = DrawPhysics.SphereCastAll(ray, 1, 100, LayerMasks.All, QueryTriggerInteraction.Ignore);

                if (results.Length == 1 && results[0].transform.gameObject.layer == LayerMasks.GroundLayer)
                {
                    randomPositionInside = results[0].point;
                    break;
                }

                iterations++;

                if (iterations > 1000)
                {
                    Debug.LogError($"Couldn't find a random point in {gameObject.name}");
                    break;
                }
            }

            return randomPositionInside;
        }

        private bool ValidRandomPosition(in Vector3 position, Collider colliderFrom)
        {
            Bounds fromBounds = colliderFrom.bounds;
            Bounds adjustedBounds = new Bounds(fromBounds.center, fromBounds.extents - new Vector3(1, 0, 1) * 5f);
            if (adjustedBounds.Contains(position))
            {
                return true;
            }

            return false;
        }
    }
}