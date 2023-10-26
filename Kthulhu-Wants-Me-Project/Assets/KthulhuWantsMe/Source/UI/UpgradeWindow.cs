using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeWindow : BaseWindow
    {
        [FormerlySerializedAs("_upgradeUIPrefab")] [SerializeField] private BranchView branchViewPrefab;
        [SerializeField] private Transform _upgradesParent;

        public Action OnUpgradePicked { get; private set; }
        
        private List<UpgradeData> _upgrades;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Init(Action onUpgradePicked)
        {
            OnUpgradePicked = onUpgradePicked;
            UpdateUI();
        }

        public void UpdateUI()
        {
            foreach (UpgradeData upgrade in _upgrades)
            {
                BranchView branchView = _gameFactory.CreatePrefabInjected(branchViewPrefab, _upgradesParent);
                branchView.Init(upgrade, this);
            }
        }
    }
}