using System.Collections;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class FadeableObject : MonoBehaviour
    {
        public Renderer Renderer;

        private bool _faded;
        private bool _inProcess;

        private float _lastRequestTime;
        
        private static readonly int DitherThreshold = Shader.PropertyToID("_DitherThreshold");
        
        private void OnValidate()
        {
            Renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (_faded && _lastRequestTime + .5f < Time.time && !_inProcess)
            {
                _faded = false;
                StartCoroutine(DoUnFadeObject(Renderer));
            }
        }

        public void RequestFade()
        {
            if (_faded || _inProcess)
            {
                _lastRequestTime = Time.time;
                return;
            }

            _faded = true;
            _lastRequestTime = Time.time;
            StartCoroutine(DoFadeObject(Renderer));
        }
        
        private IEnumerator DoFadeObject(Renderer rendererComponent)
        {
            _inProcess = true;
            for (int i = 0; i < rendererComponent.materials.Length; i++)
            {
                rendererComponent.materials[i].SetFloat(DitherThreshold, 1);
                for (float t = 1; t > .29f; t -= Time.deltaTime * 2f)
                {
                    rendererComponent.materials[i].SetFloat(DitherThreshold, t);
                    yield return null;
                }
            }
          
            _inProcess = false;
        }

        private IEnumerator DoUnFadeObject(Renderer rendererComponent)
        {
            _inProcess = true;
            for (int i = 0; i < rendererComponent.materials.Length; i++)
            {
                for (float t = rendererComponent.materials[i].GetFloat(DitherThreshold); t < 1; t += Time.deltaTime * 4f)
                {
                    rendererComponent.materials[i].SetFloat(DitherThreshold, t);
                    yield return null;
                }
                rendererComponent.materials[i].SetFloat(DitherThreshold, 1);
            }

            _inProcess = false;
        }
    }
}