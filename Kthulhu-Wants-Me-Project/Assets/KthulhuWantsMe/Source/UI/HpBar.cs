using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBarImage;
        [SerializeField] private float _hpUnit;

        [SerializeField] private TextMeshProUGUI _hpText;

        public void SetValue(float current, float max)
        {
            _hpBarImage.fillAmount = current / max;
            if(_hpText != null)
                _hpText.text = $"{current} / {max}";
        }
        
        public void SetNewMax(float newMax)
        {
            float hpBarWidth = _hpUnit * newMax;
            _hpBarImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpBarWidth);
        }
    }
}
