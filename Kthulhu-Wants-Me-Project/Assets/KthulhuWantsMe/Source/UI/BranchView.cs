using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
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
        [SerializeField] private TextMeshProUGUI _stageStats;
        
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
            _stageStats.text = string.Format(upgradeData.UpgradeText, upgradeData.Value);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _upgradeService.ApplyUpgrade(_upgradeData);
            _upgradeWindow.OnUpgradePicked?.Invoke();
            Destroy(_upgradeWindow.gameObject);
        }
    }
}