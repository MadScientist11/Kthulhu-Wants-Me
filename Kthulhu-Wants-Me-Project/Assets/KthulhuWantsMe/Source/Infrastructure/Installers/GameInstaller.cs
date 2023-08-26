using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
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
            builder
                .Register<GameFactory>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
          
            RegisterGameplayFsm(builder);
        }

        private static void RegisterGameplayFsm(IContainerBuilder builder)
        {
            builder.Register<StatesFactory>(Lifetime.Scoped);
            builder.Register<GameStateMachine>(Lifetime.Scoped);
            builder.Register<StartGameState>(Lifetime.Scoped);
        }
    }
}