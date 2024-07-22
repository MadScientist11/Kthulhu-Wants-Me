using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class UpgradeWindow : BaseWindow
    {
        public override WindowId Id => WindowId.UpgradeWindow;
        
        [FormerlySerializedAs("_upgradeUIPrefab")] [SerializeField] private BranchView branchViewPrefab;
        [SerializeField] private Transform _upgradesParent;

        public Action OnUpgradePicked { get; private set; }
        
        private List<UpgradeData> _upgrades;
        
        private IGameFactory _gameFactory;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IGameFactory gameFactory, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _gameFactory = gameFactory;
        }

        public void Init(Action onUpgradePicked)
        {
            OnUpgradePicked = onUpgradePicked;
            UpdateUI();
        }

        public void UpdateUI()
        {
            foreach (BranchTemplate branchTemplate in _dataProvider.SkillTree.SkillTree.Branches)
            {
                BranchView branchView = _gameFactory.CreateInjected(branchViewPrefab, _upgradesParent);
                branchView.Init(branchTemplate.Branch, this);
            }
        }

    }
}