using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services.DataProviders
{
    public interface IDataProvider : IInitializableService
    {
        PlayerConfiguration PlayerConfig { get; }
        PortalConfiguration PortalConfig { get; }
        BuffItemsContainer BuffItems { get; }
        Waves Waves { get; }
        EnemyConfigsProvider EnemyConfigsProvider { get; }
        GameConfiguration GameConfig { get; }
    }

    public class DataProvider : IDataProvider
    {
        public bool IsInitialized { get; set; }
        private const string PlayerConfigurationPath = "PlayerConfiguration";
        private const string PortalConfigurationPath = "PortalConfiguration";
        private const string BuffItemsPath = "BuffItemsContainer";
        private const string WavesPath = "Waves";
        private const string GameConfiguration = "GameConfiguration";
        public PlayerConfiguration PlayerConfig { get; private set; }
        public PortalConfiguration PortalConfig { get; private set; }
        public BuffItemsContainer BuffItems { get; private set; }
        public Waves Waves { get; private set; }
        
        public GameConfiguration GameConfig { get; private set; }
        public EnemyConfigsProvider EnemyConfigsProvider { get; private set; }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            InitEnemyConfigs();
            PlayerConfig = (PlayerConfiguration)await Resources.LoadAsync<PlayerConfiguration>(PlayerConfigurationPath);
            PortalConfig = (PortalConfiguration)await Resources.LoadAsync<PortalConfiguration>(PortalConfigurationPath);
            BuffItems = (BuffItemsContainer)await Resources.LoadAsync<BuffItemsContainer>(BuffItemsPath);
            Waves = (Waves)await Resources.LoadAsync<Waves>(WavesPath);
            GameConfig = (GameConfiguration)await Resources.LoadAsync<GameConfiguration>(GameConfiguration);
        }
        

        private void InitEnemyConfigs()
        {
            EnemyConfigsProvider = new EnemyConfigsProvider();
            EnemyConfigsProvider.Initialize();
        }
    }
}