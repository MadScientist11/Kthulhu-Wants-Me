using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeWindow : BaseWindow
    {
        [SerializeField] private UpgradeUI _upgradeUIPrefab;
        [SerializeField] private Transform _upgradesParent;

        public Action OnUpgradePicked { get; private set; }
        
        private List<UpgradeData> _upgrades;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

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
                UpgradeUI upgradeUI = _gameFactory.CreatePrefabInjected(_upgradeUIPrefab, _upgradesParent);
                upgradeUI.Init(upgrade, this);
            }
        }
    }
}