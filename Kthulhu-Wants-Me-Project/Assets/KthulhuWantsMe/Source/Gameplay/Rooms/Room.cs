using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Rooms
{
    public class Room : MonoBehaviour, IInjectable
    {
        [SerializeField] private List<Collider> _roomColliders;

        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

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

        public Vector3 GetRandomPositionInside(bool snapToGround = false)
        {
            Collider roomPartCollider = _roomColliders[Random.Range(0, _roomColliders.Count)];
            Vector3 randomPositionInside = RandomPosition.GetRandomPosition(roomPartCollider.bounds);
            
            if (snapToGround)
            {
                if (Physics.Raycast(
                        randomPositionInside, 
                        Vector3.down, 
                        out RaycastHit hitInfo, 
                        100,
                        LayerMasks.GroundMask))
                {
                    randomPositionInside = hitInfo.point;
                }
                else
                {
                    Debug.LogError("Couldn't find ground");
                }
            }

            return randomPositionInside;
        }
    }
}