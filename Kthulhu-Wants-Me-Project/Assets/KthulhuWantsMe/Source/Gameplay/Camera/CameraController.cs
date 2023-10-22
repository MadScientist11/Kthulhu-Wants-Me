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
    public interface ICameraFadeble
    {
    }

    public class CameraController : MonoBehaviour
    {
        public ICinemachineCamera ActiveVCam
        {
            get { return _cinemachineBrain.ActiveVirtualCamera; }
        }

        [SerializeField] private CinemachineBrain _cinemachineBrain;

        private Renderer _inCameraView;

        private RaycastHit[] _results = new RaycastHit[10];
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

            if (Physics.SphereCast(ray, 1f, out RaycastHit hit, dir.magnitude))
            {
                if (hit.transform.TryGetComponent(out FadeableObject fadeableObject))
                {
                    if ((_inCameraView == null || hit.transform != _inCameraView.transform))
                    {
                        StartCoroutine(DoFadeObject(fadeableObject.Renderer));
                        _inCameraView = fadeableObject.Renderer;
                    }
                }
                else
                {
                    if (_inCameraView != null)
                    {
                        StartCoroutine(DoUnFadeObject(_inCameraView));
                        _inCameraView = null;
                    }
                }
            }
           
          
        }

        private IEnumerator DoFadeObject(Renderer rendererComponent)
        {
            //rendererComponent.material.SetFloat("_StencilMode", 1);
            //rendererComponent.material.SetFloat("_StencilNo", 10);
            rendererComponent.material.SetFloat("_DitherThreshold", 1);
            Debug.Log(rendererComponent.transform);
            for (float t = 1; t > .29f; t -= Time.deltaTime * 2f)
            {
                rendererComponent.material.SetFloat("_DitherThreshold", t);
                yield return null;
            }
        }

        private IEnumerator DoUnFadeObject(Renderer rendererComponent)
        {
            yield return new WaitForSeconds(0.5f);
            //rendererComponent.material.SetFloat("_StencilMode", 0);
            //rendererComponent.material.SetFloat("_StencilNo", 1);
            for (float t = rendererComponent.material.GetFloat("_DitherThreshold"); t < 1; t += Time.deltaTime * 2f)
            {
                rendererComponent.material.SetFloat("_DitherThreshold", t);
                yield return null;
            }

            rendererComponent.material.SetFloat("_DitherThreshold", 1);
        }
    }
}