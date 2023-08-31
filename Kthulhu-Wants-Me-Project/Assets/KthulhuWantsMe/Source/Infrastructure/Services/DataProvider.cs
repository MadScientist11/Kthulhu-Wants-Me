using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Portal;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IDataProvider
    {
        PlayerConfiguration PlayerConfig { get; }
        TentacleConfiguration TentacleConfig { get; }
        MinionConfiguration MinionConfig { get; }
        PortalConfiguration PortalConfig { get; }
    }

    public class DataProvider : IDataProvider, IInitializableService
    {
        public bool IsInitialized { get; set; }
        private const string PlayerConfigurationPath = "PlayerConfiguration";
        private const string TentacleConfigurationPath = "TentacleConfiguration";
        private const string PortalConfigurationPath = "PortalConfiguration";
        private const string MinionConfigurationPath = "MinionConfiguration";
        public PlayerConfiguration PlayerConfig { get; private set; }
        public TentacleConfiguration TentacleConfig { get; private set; }
        
        public MinionConfiguration MinionConfig { get; private set; }
        public PortalConfiguration PortalConfig { get; private set; }
        

        public async UniTask Initialize()
        {
            IsInitialized = true;
            PlayerConfig = (PlayerConfiguration)await Resources.LoadAsync<PlayerConfiguration>(PlayerConfigurationPath);
            TentacleConfig = (TentacleConfiguration)await Resources.LoadAsync<TentacleConfiguration>(TentacleConfigurationPath);
            PortalConfig = (PortalConfiguration)await Resources.LoadAsync<PortalConfiguration>(PortalConfigurationPath);
            MinionConfig = (MinionConfiguration)await Resources.LoadAsync<MinionConfiguration>(MinionConfigurationPath);
        }
    }
}