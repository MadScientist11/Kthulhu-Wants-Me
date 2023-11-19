using KthulhuWantsMe.Source.Infrastructure.Services;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class IndicatorsUI : MonoBehaviour
    {
        [SerializeField] private HUDNavigationSystem _hudNavigationSystem;
        [SerializeField] private HUDNavigationCanvas _hudNavigationCanvas;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        public void Enable()
        {
            HUDNavigationSystem.Instance.ChangePlayerController(_gameFactory.Player.transform);
            HUDNavigationSystem.Instance.EnableSystem(true);
        }
    }   
}