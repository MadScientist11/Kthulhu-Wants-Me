using System;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay
{
    public class Test : MonoBehaviour
    {
        private IResourceManager _resourceManager;
        private ICoroutineRunner _coroutineRunner;
        private IInputService _inputService;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner, IResourceManager resourceManager, IInputService inputService)
        {
            _inputService = inputService;
            _coroutineRunner = coroutineRunner;
            _resourceManager = resourceManager;
        }

        private void Start()
        {
            Debug.Log(_resourceManager);
            Debug.Log(_coroutineRunner);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
        }

        private void Update()
        {
            Debug.Log(_inputService.GameplayScenario?.MovementInput);
        }
    }
}