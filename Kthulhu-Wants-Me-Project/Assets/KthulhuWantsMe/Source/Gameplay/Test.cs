using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay
{
    public class Test : MonoBehaviour
    {
        private IResourceManager _resourceManager;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner, IResourceManager resourceManager)
        {
            _coroutineRunner = coroutineRunner;
            _resourceManager = resourceManager;
        }

        private void Start()
        {
            Debug.Log(_resourceManager);
            Debug.Log(_coroutineRunner);
        }
    }
}