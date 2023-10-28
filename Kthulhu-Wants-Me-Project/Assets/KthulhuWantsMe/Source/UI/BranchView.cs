using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class BranchView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _branchText;
        [SerializeField] private TextMeshProUGUI _stageStats;
        
        private UpgradeData _upgradeData;
        private UpgradeWindow _upgradeWindow;
        
        private IUpgradeService _upgradeService;
        private IProgressService _progressService;
        private Branch _branch;

        [Inject]
        public void Construct(IUpgradeService upgradeService, IProgressService progressService)
        {
            _progressService = progressService;
            _upgradeService = upgradeService;
        }

        public void Init(Branch branch, UpgradeWindow upgradeWindow)
        {
            _branch = branch;
            _upgradeWindow = upgradeWindow;
            _branchText.text = "Aggressor";

            int completedStage = _progressService.ProgressData.CompletedSkillBranchStages.GetOrCreate(branch.InstanceId);
            BranchStage branchBranchStage = branch.BranchStages[completedStage].BranchStage;
            
            string stats = string.Empty;
            foreach (UpgradeData upgradeData in branchBranchStage.Upgrades)
            {
                stats += upgradeData.UpgradeText + "\n";
            }
            _stageStats.text = stats;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click!");
            int completedStage = _progressService.ProgressData.CompletedSkillBranchStages.GetOrCreate(_branch.InstanceId);
            BranchStage branchStage = _branch.BranchStages[completedStage].BranchStage;
            foreach (UpgradeData upgradeData in branchStage.Upgrades)
            {
                _upgradeService.ApplyUpgrade(upgradeData);
            }

            _progressService.ProgressData.CompletedSkillBranchStages[_branch.InstanceId]++;
            
            _upgradeWindow.OnUpgradePicked?.Invoke();
            Destroy(_upgradeWindow.gameObject);
        }
    }
}