using KthulhuWantsMe.Source.Gameplay.Player;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly PlayerSpawnPoint _playerSpawnPoint;

        public GameInstaller(PlayerSpawnPoint playerSpawnPoint)
        {
            _playerSpawnPoint = playerSpawnPoint;
        }
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_playerSpawnPoint);
        }
    }
}