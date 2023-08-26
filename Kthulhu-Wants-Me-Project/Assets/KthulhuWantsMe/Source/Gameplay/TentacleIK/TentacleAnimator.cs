using System;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class TentacleAnimator : MonoBehaviour
    {
        private bool GrabPlayerAnimationActive
        {
            get => _grabPlayerAnimationActive;
            set
            {
                _tentacleMaterial.SetInt(GrabPlayer, Convert.ToInt32(value));
                _grabPlayerAnimationActive = value;
            }
        }

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Vector3 _tentacleGrabOffset;
        [SerializeField] private float _tentacleGrabRadius;
        [SerializeField] private float _twirlStrength;

        private Material _tentacleMaterial;
        private Transform _playerFollowTarget;
        private bool _grabPlayerAnimationActive;

        private static readonly int GrabPlayer = Shader.PropertyToID("_GrabPlayer");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int InteractPos = Shader.PropertyToID("_InteractPos");
        private static readonly int TwirlStrength = Shader.PropertyToID("_TwirlStrength");

        private void Start()
        {
            _tentacleMaterial = _renderer.material;
        }

        private void Update()
        {
            if (GrabPlayerAnimationActive)
            {
                _tentacleMaterial.SetFloat(Radius, _tentacleGrabRadius);
                _tentacleMaterial.SetFloat(TwirlStrength, _twirlStrength);
                _tentacleMaterial.SetVector(InteractPos, _playerFollowTarget.localPosition + _tentacleGrabOffset);
            }
        }

        public void PlayGrabPlayerAnimation(Transform playerFollowTarget)
        {
            _playerFollowTarget = playerFollowTarget;
            GrabPlayerAnimationActive = true;
        }

        public void PlayIdleAnimation()
        {
            GrabPlayerAnimationActive = false;
        }
    }
}