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

        [Inject]
        public void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }


        private void Update()
        {
            if (!_unlocked && _progressService.ProgressData.CompletedWaveIndex >= _unlockOnWave - 1)
            {
                _unlocked = true;
                gameObject.SetActive(false);
            }
        }
    }
}
