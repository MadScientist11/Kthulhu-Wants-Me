using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [ExecuteInEditMode]
    public class TentacleGrabTest : MonoBehaviour
    {
        [SerializeField] private Renderer _tentacleRenderer;

        [SerializeField] private Vector3 _tentacleGrabOffset;
        [SerializeField] private float _tentacleGrabRadius;
        [SerializeField] private float _twirlStrength;

        [SerializeField] private Transform _grabTarget;
        
        private Material _tentacleMaterial;
        private bool _grabPlayerAnimationActive;

        private static readonly int GrabPlayerParam = Shader.PropertyToID("_EnableTentacleGrab");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int InteractPos = Shader.PropertyToID("_InteractPos");
        private static readonly int TwirlStrength = Shader.PropertyToID("_TwirlStrength");

        private void OnValidate()
        {
            _tentacleMaterial = _tentacleRenderer.sharedMaterial;
            _tentacleMaterial.SetInt(GrabPlayerParam, 1);
            
        }

        private void Update()
        {
            _tentacleMaterial.SetFloat(Radius, _tentacleGrabRadius);
            _tentacleMaterial.SetFloat(TwirlStrength, _twirlStrength);
            _tentacleMaterial.SetVector(InteractPos, _grabTarget.localPosition + _tentacleGrabOffset);
        }
    }
}
