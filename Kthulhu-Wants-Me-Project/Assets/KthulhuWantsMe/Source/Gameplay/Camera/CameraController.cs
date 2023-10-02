using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1000f))
            {
                if (hit.transform.TryGetComponent(out Renderer rendererComponent))
                {
                    if (rendererComponent.material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.Transparent && (_inCameraView == null ||hit.transform != _inCameraView.transform))
                    {
                        StartCoroutine(DoFadeObject(rendererComponent));
                    
                        _inCameraView = rendererComponent;
                    }
                }
                
                if (_inCameraView != null && hit.transform != _inCameraView.transform )
                {
                    StartCoroutine(DoUnFadeObject(_inCameraView));
                    _inCameraView = null;
                }

            }
        }

        private IEnumerator DoFadeObject(Renderer rendererComponent)
        {
            rendererComponent.material.SetFloat("_StencilMode", 1);
            rendererComponent.material.SetFloat("_StencilNo", 10);
            for (float t = 0; t > -.75f; t-= Time.deltaTime*2f)
            {
                rendererComponent.material.SetFloat("_Tweak_transparency", t);
                yield return null;
            }
        }
        
        private IEnumerator DoUnFadeObject(Renderer rendererComponent)
        {
            
            rendererComponent.material.SetFloat("_StencilMode", 0);
            rendererComponent.material.SetFloat("_StencilNo", 1);
            for (float t = rendererComponent.material.GetFloat("_Tweak_transparency"); t < 0; t+= Time.deltaTime*2f)
            {
                rendererComponent.material.SetFloat("_Tweak_transparency", t);
                yield return null;
            }
            rendererComponent.material.SetFloat("_Tweak_transparency", 0);
        }
    }
}