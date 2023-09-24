using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService : IInitializableService
    {
    }

    public class UIService : IUIService
    {
        public bool IsInitialized { get; set; }
        
        public UniTask Initialize()
        {
            IsInitialized = true;
            return UniTask.CompletedTask;
        }

        public void OpenWindow(WindowId windowId)
        {
            
        }

        public void OpenPopUp(PopUpId popUpId)
        {
            
        }
        
        public void ShowNotification() { }

        public void ShowPlayerHUD()
        {
            
        }
        
        public void HidePlayerHUD()
        {
            
        }
    }
}