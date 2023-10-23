using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        public ICinemachineCamera ActiveVCam
        {
            get
            {
                return _cinemachineBrain.ActiveVirtualCamera;
            }
        }

        [SerializeField] private CinemachineBrain _cinemachineBrain;

        private IGameFactory _gameFactory;
        [SerializeField] private float _offset = 1f;
        [SerializeField] private float _radius = 0.5f;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start() =>
            Cursor.lockState = CursorLockMode.Confined;


        private void Update()
        {
            FadeObjectsInView();
        }

        public void PullFocusOn(ICinemachineCamera cinemachineCamera)
        {
            if (cinemachineCamera == ActiveVCam)
                return;

            ActiveVCam.VirtualCameraGameObject.SetActive(false);
            cinemachineCamera.VirtualCameraGameObject.SetActive(true);
        }

        private void FadeObjectsInView()
        {
            Vector3 dir = _gameFactory.Player.transform.position - (transform.position - transform.up * _offset);
            Ray ray = new Ray(transform.position - transform.up * _offset, dir.normalized);

            if (DrawPhysics.SphereCast(ray, _radius, out RaycastHit hit, dir.magnitude, LayerMask.NameToLayer(GameConstants.Layers.Player)))
            {
                if (hit.transform.TryGetComponent(out FadeableObject fadeableObject))
                {
                    fadeableObject.RequestFade();
                }
            }
        }
    }
}