using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI.Window
{
    public abstract class BaseWindow : MonoBehaviour, IUIElement
    {
        public abstract WindowId Id { get; }
        
        private IUIService _uiService;

        [Inject]
        public void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }
        
        public void Init()
        {
            
        }

        public void Hide()
        {
            _uiService.CloseWindow(Id);
        }
    }
}