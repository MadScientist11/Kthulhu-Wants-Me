using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public interface IStateResetter
    {
        void ResetState();
    }

    public interface IStoppable
    {
        void StopEntityLogic();
        void ResumeEntityLogic();
    }
    
    public class CyaeghaFacade : MonoBehaviour, IStateResetter, IStoppable
    {
        [SerializeField] private CyaeghaAIBrain _cyaeghaAIBrain;
        [SerializeField] private FollowLogic _followLogic;
        
        public void ResetState()
        {
            
        }

        public void StopEntityLogic()
        {
            _followLogic.DisableMotor();
            BlockAIProcessing();
        }

        public void ResumeEntityLogic()
        {
            _followLogic.EnableMotor();
            ResumeAIProcessing();
        }

        public void ResetBrain() =>
            _cyaeghaAIBrain.ResetAI();

        public void BlockAIProcessing()
            => _cyaeghaAIBrain.BlockProcessing = true;
        
        public void ResumeAIProcessing()
            => _cyaeghaAIBrain.BlockProcessing = false;
    }
}