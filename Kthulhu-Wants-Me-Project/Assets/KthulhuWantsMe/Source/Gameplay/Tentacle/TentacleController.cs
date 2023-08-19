using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Tentacle
{
    [RequireComponent(typeof(Renderer))]
    public class TentacleController : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;
        
        [SerializeField] private TriggerZone _triggerZone;

        private Material _tentacleMaterial;
        
        private void Awake()
        {
            _tentacleMaterial = _renderer.material;
            _triggerZone.OnTrigger += EnableTentacleGrab;
        }

        private void Update()
        {
            float radius = _player.localScale.x - 2f;
            _tentacleMaterial.SetFloat("_Radius", radius);
            _tentacleMaterial.SetVector("_InteractPos", transform.InverseTransformPoint(_player.position + _offset * radius));
        }

        private void EnableTentacleGrab()
        {
            _tentacleMaterial.SetInt("_EnableTentacleGrab", 1);
        }
    }
}