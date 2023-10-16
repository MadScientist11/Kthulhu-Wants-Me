using TMPro;
using UnityEngine;

namespace KthulhuWantsMe.Source.UI
{
    public class MiscUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveCountdownText;
     
        public void UpdateWaveCountdownText(int countdown)
        {
            _waveCountdownText.text = countdown.ToString();
        }
    }
}