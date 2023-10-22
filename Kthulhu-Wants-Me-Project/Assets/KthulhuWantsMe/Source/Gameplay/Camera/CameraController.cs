using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

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
            Vector3 dir = _gameFactory.Player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir.normalized);

            if (Physics.SphereCast(ray, 2f, out RaycastHit hit, dir.magnitude))
            {
                if (hit.transform.TryGetComponent(out FadeableObject fadeableObject))
                {
                    fadeableObject.RequestFade();
                }
            }
        }
    }
}