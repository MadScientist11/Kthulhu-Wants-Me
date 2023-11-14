using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class BranchView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _availabilityPanel;
        [SerializeField] private TextMeshProUGUI _branchText;
        [SerializeField] private TextMeshProUGUI _stageStats;
        [SerializeField] private Image _skillImage;
        [SerializeField] private List<Image> _points;

        [SerializeField] private MMFeedbacks _buttonFeedback;
        
        private UpgradeData _upgradeData;
        private UpgradeWindow _upgradeWindow;
        private Branch _branch;
        
        
        private IUpgradeService _upgradeService;
        private IProgressService _progressService;
        private IUIService _uiService;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(IUpgradeService upgradeService, IProgressService progressService, IUIService uiService, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _uiService = uiService;
            _progressService = progressService;
            _upgradeService = upgradeService;
        }

        public void Init(Branch branch, UpgradeWindow upgradeWindow)
        {
            _branch = branch;
            _upgradeWindow = upgradeWindow;
            _branchText.text = branch.BranchName;

            int completedStage = _progressService.ProgressData.CompletedSkillBranchStages.GetOrCreate(branch.InstanceId);

            if (completedStage >= branch.BranchStages.Count)
            {
                _availabilityPanel.SetActive(true);
            }
            completedStage = Mathf.Clamp(completedStage, 0, branch.BranchStages.Count - 1);
            
            BranchStage branchBranchStage = branch.BranchStages[completedStage].BranchStage;
            
            for (int i = 0; i < completedStage; i++)
            {
                _points[i].color = Color.green;
            }
            string stats = string.Empty;
            foreach (UpgradeData upgradeData in branchBranchStage.Upgrades)
            {
                if (upgradeData.UpgradeType == UpgradeType.StatUpgrade)
                {
                    stats += $"{upgradeData.StatType.ToString().SplitCamelCase()} [{upgradeData.Value}] \n";
                }
                else if (upgradeData.UpgradeType == UpgradeType.SkillAcquirement)
                {
                    _skillImage.sprite = _dataProvider.AllSkills[upgradeData.SkillId].Sprite;
                }
            }
            _stageStats.text = stats;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int completedStage = _progressService.ProgressData.CompletedSkillBranchStages.GetOrCreate(_branch.InstanceId);
            if (completedStage >= _branch.BranchStages.Count)
            {
               return;
            }
            BranchStage branchStage = _branch.BranchStages[completedStage].BranchStage;
            foreach (UpgradeData upgradeData in branchStage.Upgrades)
            {
                _upgradeService.ApplyUpgrade(upgradeData);
            }
            _buttonFeedback?.PlayFeedbacks();
            _progressService.ProgressData.CompletedSkillBranchStages[_branch.InstanceId]++;
            
            _upgradeWindow.OnUpgradePicked?.Invoke();
            Destroy(_upgradeWindow.gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _uiService.Tooltip.Show("");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}