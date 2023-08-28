using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Interactions;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
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
                .Register<GameFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<InteractionsManager>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<InventorySystem>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<PlayerStats>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            RegisterGameplayFsm(builder);
        }

        private static void RegisterGameplayFsm(IContainerBuilder builder)
        {
            builder.Register<StatesFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<StartGameState>(Lifetime.Singleton);
        }
    }
}