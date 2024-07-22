using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.StaterReset
{
    public class State : MonoBehaviour
    {
        public void ResetState()
        {
            foreach (IStateReset resettable in GetComponents<IStateReset>())
            {
                resettable.ResetState();
            }
        }

        public void PauseLogic()
        {
            foreach (IStateLogic resettable in GetComponents<IStateLogic>())
            {
                resettable.PauseLogic();
            }
        }

        public void ResumeLogic()
        {
            foreach (IStateLogic resettable in GetComponents<IStateLogic>())
            {
                resettable.ResumeLogic();
            }
        }
    }
}