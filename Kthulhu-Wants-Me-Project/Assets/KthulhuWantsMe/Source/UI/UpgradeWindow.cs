using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Upgrades;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeWindow : BaseWindow
    {
        [SerializeField] private UpgradeUI _upgradeUIPrefab;
        [SerializeField] private Transform _upgradesParent;
        
        private List<IUpgrade> _upgrades;
        
        public Action OnUpgradePicked { get; private set; }

        public void Init(List<IUpgrade> upgrades, Action onUpgradePicked)
        {
            OnUpgradePicked = onUpgradePicked;
            _upgrades = upgrades;
            UpdateUI(upgrades);
        }

        public void UpdateUI(List<IUpgrade> upgrades)
        {
            foreach (IUpgrade upgrade in upgrades)
            {
                UpgradeUI upgradeUI = Instantiate(_upgradeUIPrefab, _upgradesParent);
                upgradeUI.Init(upgrade, this);
            }
        }
    }
}