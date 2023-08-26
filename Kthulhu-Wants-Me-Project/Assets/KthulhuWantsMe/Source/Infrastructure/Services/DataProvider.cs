using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IDataProvider
    {
        PlayerConfiguration PlayerConfig { get; }
    }

    public class DataProvider : IDataProvider, IInitializableService
    {
        public bool IsInitialized { get; set; }
        private const string PlayerConfigurationPath = "PlayerConfiguration";
        public PlayerConfiguration PlayerConfig { get; private set; }
        

        public async UniTask Initialize()
        {
            IsInitialized = true;
            PlayerConfig = (PlayerConfiguration)await Resources.LoadAsync<PlayerConfiguration>(PlayerConfigurationPath);
        }
    }
}