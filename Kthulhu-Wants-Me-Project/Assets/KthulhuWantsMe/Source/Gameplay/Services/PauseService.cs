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
        private bool _gamePaused;
        private BaseWindow _settingsWindow;
        
        
        public void PauseGame()
        {
            Time.timeScale = 0;
            _gamePaused = true;
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1;
            _gamePaused = false;
        }
    }
}