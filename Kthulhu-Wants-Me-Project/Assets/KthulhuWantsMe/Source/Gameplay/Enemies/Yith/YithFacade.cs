using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithFacade : MonoBehaviour, IStoppable, IStateResetter
    {
        [SerializeField] private YithAIBrain _yithAIBrain;
        [SerializeField] private MovementMotor _movementMotor;
        
        public void ResetState()
        {
            
        }

        public void StopEntityLogic()
        {
            _movementMotor.Disable();
            BlockAIProcessing();
        }

        public void ResumeEntityLogic()
        {
            _movementMotor.Enable();
            ResumeAIProcessing();
        }

        //public void ResetBrain() =>
        //    _yithAIBrain.ResetAI();

        public void BlockAIProcessing()
            => _yithAIBrain.BlockProcessing = true;
        
        public void ResumeAIProcessing()
            => _yithAIBrain.BlockProcessing = false;
    }
}