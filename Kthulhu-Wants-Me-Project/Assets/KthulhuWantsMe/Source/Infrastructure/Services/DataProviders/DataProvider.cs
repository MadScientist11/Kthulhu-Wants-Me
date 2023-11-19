using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
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
        WavesConfiguration Waves { get; }
        EnemyConfigsProvider EnemyConfigsProvider { get; }
        GameConfiguration GameConfig { get; }
        SkillTreeTemplate SkillTree { get; }
        AllSkills AllSkills { get; }
    }

    public class DataProvider : IDataProvider
    {
        public bool IsInitialized { get; set; }
        private const string PlayerConfigurationPath = "PlayerConfiguration";
        private const string PortalConfigurationPath = "PortalConfiguration";
        private const string BuffItemsPath = "BuffItemsContainer";
        private const string WavesPath = "WavesConfiguration";
        private const string GameConfiguration = "GameConfiguration";
        private const string SkillTreePath = "SkillTree";
        public PlayerConfiguration PlayerConfig { get; private set; }
        public PortalConfiguration PortalConfig { get; private set; }
        public BuffItemsContainer BuffItems { get; private set; }
        public WavesConfiguration Waves { get; private set; }
        
        public GameConfiguration GameConfig { get; private set; }
        public EnemyConfigsProvider EnemyConfigsProvider { get; private set; }
        public SkillTreeTemplate SkillTree { get; private set; }
        
        public AllSkills AllSkills { get; private set; }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            InitEnemyConfigs();
            PlayerConfig = (PlayerConfiguration)await Resources.LoadAsync<PlayerConfiguration>(PlayerConfigurationPath);
            PortalConfig = (PortalConfiguration)await Resources.LoadAsync<PortalConfiguration>(PortalConfigurationPath);
            BuffItems = (BuffItemsContainer)await Resources.LoadAsync<BuffItemsContainer>(BuffItemsPath);
            Waves = (WavesConfiguration)await Resources.LoadAsync<WavesConfiguration>(WavesPath);
            GameConfig = (GameConfiguration)await Resources.LoadAsync<GameConfiguration>(GameConfiguration);
            SkillTree = (SkillTreeTemplate)await Resources.LoadAsync<SkillTreeTemplate>(SkillTreePath);
            AllSkills = new();
        }
        

        private void InitEnemyConfigs()
        {
            EnemyConfigsProvider = new EnemyConfigsProvider();
            EnemyConfigsProvider.Initialize();
        }
    }
}