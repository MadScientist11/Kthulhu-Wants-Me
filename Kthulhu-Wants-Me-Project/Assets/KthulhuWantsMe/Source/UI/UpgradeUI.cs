using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _upgradeDescription;
        [SerializeField] private Button _upgradeButton;
        
        private UpgradeData _upgradeData;
        private UpgradeWindow _upgradeWindow;
        
        private IUpgradeService _upgradeService;

        [Inject]
        public void Construct(IUpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
        }

        public void Init(UpgradeData upgradeData, UpgradeWindow upgradeWindow)
        {
            _upgradeWindow = upgradeWindow;
            _upgradeData = upgradeData;
            _upgradeDescription.text = string.Format(upgradeData.UpgradeText, upgradeData.Value);
            _upgradeButton.onClick.AddListener(OnUpgrade);
        }

        private void OnDestroy()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgrade);

        }

        private void OnUpgrade()
        {
            _upgradeService.ApplyUpgrade(_upgradeData);
            _upgradeWindow.OnUpgradePicked?.Invoke();
            Destroy(_upgradeWindow.gameObject);
        }
    }
}