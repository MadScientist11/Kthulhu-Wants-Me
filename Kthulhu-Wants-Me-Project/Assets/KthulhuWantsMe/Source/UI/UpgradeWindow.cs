using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeWindow : BaseWindow
    {
        [SerializeField] private UpgradeUI _upgradeUIPrefab;
        [SerializeField] private Transform _upgradesParent;

        public Action OnUpgradePicked { get; private set; }
        
        private List<UpgradeData> _upgrades;

        public void Init(List<UpgradeData> upgrades, Action onUpgradePicked)
        {
            OnUpgradePicked = onUpgradePicked;
            _upgrades = upgrades;
            UpdateUI();
        }

        public void UpdateUI()
        {
            foreach (UpgradeData upgrade in _upgrades)
            {
                UpgradeUI upgradeUI = Instantiate(_upgradeUIPrefab, _upgradesParent);
                upgradeUI.Init(upgrade, this);
            }
        }
    }
}