using System;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay
{
    public class FogWall : MonoBehaviour, IInjectable
    {
        [SerializeField] private int _unlockOnWave;
        
        
        private IProgressService _progressService;
        private bool _unlocked;

        [Inject]
        public void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }


        private void Update()
        {
            if (!_unlocked && _progressService.ProgressData.DefeatedWaveIndex >= _unlockOnWave)
            {
                _unlocked = true;
                gameObject.SetActive(false);
            }
        }
    }
}
