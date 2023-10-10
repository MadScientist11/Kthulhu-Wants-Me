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

            if (Physics.SphereCast(ray, 1f, out RaycastHit hit, dir.magnitude, LayerMasks.FadeableObjectMask))
            {
                if (hit.transform.TryGetComponent(out Renderer rendererComponent))
                {
                    if (rendererComponent.material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.Transparent &&
                        (_inCameraView == null || hit.transform != _inCameraView.transform))
                    {
                        StartCoroutine(DoFadeObject(rendererComponent));
                        _inCameraView = rendererComponent;
                    }
                }

                return;

               
            }
            if (_inCameraView != null)
            {
                StartCoroutine(DoUnFadeObject(_inCameraView));
                _inCameraView = null;
            }
          
        }

        private IEnumerator DoFadeObject(Renderer rendererComponent)
        {
            rendererComponent.material.SetFloat("_StencilMode", 1);
            rendererComponent.material.SetFloat("_StencilNo", 10);
            for (float t = 0; t > -.75f; t -= Time.deltaTime * 2f)
            {
                rendererComponent.material.SetFloat("_Tweak_transparency", t);
                yield return null;
            }
        }

        private IEnumerator DoUnFadeObject(Renderer rendererComponent)
        {
            rendererComponent.material.SetFloat("_StencilMode", 0);
            rendererComponent.material.SetFloat("_StencilNo", 1);
            for (float t = rendererComponent.material.GetFloat("_Tweak_transparency"); t < 0; t += Time.deltaTime * 2f)
            {
                rendererComponent.material.SetFloat("_Tweak_transparency", t);
                yield return null;
            }

            rendererComponent.material.SetFloat("_Tweak_transparency", 0);
        }
    }
}