using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithFacade : MonoBehaviour, IStoppable, IStateResetter
    {
        [SerializeField] private YithAIBrain _yithAIBrain;
        [SerializeField] private FollowLogic _followLogic;
        
        public void ResetState()
        {
            
        }

        public void StopEntityLogic()
        {
            //_followLogic.DisableMotor();
            // BlockAIProcessing();
        }

        public void ResumeEntityLogic()
        {
//            _followLogic.EnableMotor();
            //ResumeAIProcessing();
        }

        //public void ResetBrain() =>
        //    _yithAIBrain.ResetAI();

        public void BlockAIProcessing()
            => _yithAIBrain.BlockProcessing = true;
        
        public void ResumeAIProcessing()
            => _yithAIBrain.BlockProcessing = false;
    }
}