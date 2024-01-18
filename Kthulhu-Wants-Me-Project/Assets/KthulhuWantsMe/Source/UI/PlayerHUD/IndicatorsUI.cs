using KthulhuWantsMe.Source.Gameplay.Services;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class IndicatorsUI : MonoBehaviour
    {
        [SerializeField] private HUDNavigationSystem _hudNavigationSystem;
        [SerializeField] private HUDNavigationCanvas _hudNavigationCanvas;
        
        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }
        
        public void Enable()
        {
            HUDNavigationSystem.Instance.ChangePlayerController(_playerProvider.Player.transform);
            HUDNavigationSystem.Instance.EnableSystem(true);
        }
    }   
}