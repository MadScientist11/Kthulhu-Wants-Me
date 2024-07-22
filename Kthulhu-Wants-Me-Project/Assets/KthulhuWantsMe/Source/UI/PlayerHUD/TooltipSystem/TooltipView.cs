using KthulhuWantsMe.Source.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI.PlayerHUD.TooltipSystem
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;

        [SerializeField] private LayoutElement _layout;

        private RectTransform _transform;
        
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _transform.position = Input.mousePosition;
        }

        public void Display(string content, string header = null)
        {
            _contentText.text = content;
            DisplayHeader(header);
            EnableWrapIfNeeded();
        }

        private void DisplayHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                _headerText.gameObject.SwitchOn();
                _headerText.text = header;
            }
            else
            {
                _headerText.gameObject.SwitchOff();
            }
        }

        private void EnableWrapIfNeeded()
        {
            if (_transform.rect.width >= _layout.preferredWidth)
                _layout.SwitchOn();
            else
                _layout.SwitchOff();
        }
    }
}
