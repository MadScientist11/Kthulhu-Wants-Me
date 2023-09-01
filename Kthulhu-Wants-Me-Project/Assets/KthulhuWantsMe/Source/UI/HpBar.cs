using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBarImage;
        
        public void SetValue(float current, float max)
        {
            _hpBarImage.fillAmount = current / max;
        }
    }
}
