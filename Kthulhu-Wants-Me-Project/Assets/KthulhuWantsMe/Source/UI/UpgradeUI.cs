using System;
using KthulhuWantsMe.Source.Gameplay.Upgrades;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _upgradeDescription;
        [SerializeField] private Button _upgradeButton;
        
        private IUpgrade _upgrade;
        private UpgradeWindow _upgradeWindow;

        public void Init(IUpgrade upgrade, UpgradeWindow upgradeWindow)
        {
            _upgradeWindow = upgradeWindow;
            _upgrade = upgrade;
            _upgradeDescription.text = upgrade.UpgradeInfo.Description;
            _upgradeButton.onClick.AddListener(OnUpgrade);
        }

        private void OnDestroy()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgrade);

        }

        private void OnUpgrade()
        {
            _upgrade.DoUpgrade();
            Destroy(_upgradeWindow.gameObject);
            _upgradeWindow.OnUpgradePicked?.Invoke();
        }
    }
}