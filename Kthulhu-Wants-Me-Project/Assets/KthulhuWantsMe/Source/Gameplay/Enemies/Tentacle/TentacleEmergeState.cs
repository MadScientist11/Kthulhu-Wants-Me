using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public sealed class TentacleEmergeState : EmergeState
    {
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleFacade _tentacleFacade;
        
        protected override void OnEmerge()
        {
            _tentacleFacade.ResetState();
            _tentacleFacade.BlockAIProcessing();
            _tentacleAnimator.PlayEmerge();
        }

        protected override void OnEmerged()
        {
            _tentacleAnimator.SetEmerged();
            _tentacleFacade.ResumeAIProcessing();
        }
    }
}