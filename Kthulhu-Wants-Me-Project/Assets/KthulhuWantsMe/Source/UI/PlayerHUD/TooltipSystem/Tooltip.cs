using KthulhuWantsMe.Source.Gameplay;
using UnityEngine;

namespace KthulhuWantsMe.Source.UI.PlayerHUD.TooltipSystem
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TooltipView _tooltipView;

        public void Show(string content, string header = null)
        {
            _tooltipView.SwitchOnGameObject();
            _tooltipView.Display(content, header);
        }
        
        public void Hide()
        {
            _tooltipView.SwitchOffGameObject();
        }
    }
}
