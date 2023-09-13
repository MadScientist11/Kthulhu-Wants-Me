using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleFacade : MonoBehaviour
    {
        [field: SerializeField] public TentacleAnimator TentacleAnimator { get; private set; }
        [field: SerializeField] public TentacleAIBrain TentacleAIBrain { get; private set; }
        [field: SerializeField] public TentacleHealth TentacleHealth { get; private set; }
        [field: SerializeField] public TentacleSpellCastingAbility TentacleSpellCastingAbility { get; private set; } 
        
        [field: SerializeField] public Transform GrabTarget { get; private set; }

        public void ResetState()
        {
            ResetBrain();
            ResetAnimator();
            RestoreHealth();
        }

        private void RestoreHealth() => 
            TentacleHealth.Revive();

        public void ResetAnimator() => 
            TentacleAnimator.ResetAnimator();

        public void ResetBrain() =>
            TentacleAIBrain.ResetAI();

        public void BlockAIProcessing()
            => TentacleAIBrain.BlockProcessing = true;
        
        public void ResumeAIProcessing()
            => TentacleAIBrain.BlockProcessing = false;

        public void CancelActiveSpells() => 
            TentacleSpellCastingAbility.CancelActiveSpells();
    }
}