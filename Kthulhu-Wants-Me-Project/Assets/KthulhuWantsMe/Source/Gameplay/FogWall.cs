using System;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay
{
    public class FogWall : MonoBehaviour, IInjectable
    {
        [SerializeField] private int _unlockOnWave;
        
        private bool _unlocked;
        
        private IProgressService _progressService;
        private IWaveSystemDirector _waveSystemDirector;

        [Inject]
        public void Construct(IProgressService progressService, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _progressService = progressService;

            _waveSystemDirector.WaveCompleted += UnlockArea;
        }

        private void OnDestroy()
        {
            _waveSystemDirector.WaveCompleted -= UnlockArea;
        }

        private void UnlockArea()
        {
            if (!_unlocked && _progressService.ProgressData.CompletedWaveIndex >= _unlockOnWave - 1)
            {
                _unlocked = true;
                gameObject.SetActive(false);
            }
        }
    }
}
