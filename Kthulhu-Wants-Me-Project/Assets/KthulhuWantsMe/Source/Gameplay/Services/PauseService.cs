using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPauseService
    {
        void ResumeGame();
        void PauseGame();
    }

    public class PauseService : IPauseService
    {
        public void PauseGame()
        {
            Time.timeScale = 0;
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}