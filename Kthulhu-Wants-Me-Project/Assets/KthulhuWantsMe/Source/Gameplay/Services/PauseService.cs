using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
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
        private BaseWindow _settingsWindow;
        
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