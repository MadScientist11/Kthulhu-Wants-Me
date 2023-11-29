using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Game;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.InGameConsole;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Rooms;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Installers
{
    public class GameInstaller : IInstaller
    {
        private readonly SceneDataProvider _sceneDataProvider;

        public GameInstaller(SceneDataProvider sceneDataProvider)
        {
            _sceneDataProvider = sceneDataProvider;
        }
        
        public void Install(IContainerBuilder builder)
        {
            builder
                .Register<GameFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemiesAIBrainService>(Lifetime.Singleton)
                .AsImplementedInterfaces();


            builder
                .RegisterComponent(_sceneDataProvider)
                .AsImplementedInterfaces();


            builder.Register<ThePlayer>(Lifetime.Singleton)
                .As<IInitializable>()
                .As<ITickable>()
                .AsSelf();
            
            builder
                .Register<LootService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<RoomOverseer>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            

            
            builder
                .Register<UpgradeService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            

            builder
                .Register<PortalFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<BuffDebuffService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<BuffDebuffFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemyStatsScalingService>(Lifetime.Singleton)
                .AsSelf();
            
            builder
                .Register<GameApp>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemyStatsProvider>(Lifetime.Singleton)
                .AsSelf();
            
            builder
                .Register<WaveSystemDirector>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<ProjectileArcFactory>(Lifetime.Singleton)
                .AsSelf();
            
            RegisterGameplayFsm(builder);
        }

        private static void RegisterGameplayFsm(IContainerBuilder builder)
        {
            builder.Register<StatesFactory>(Lifetime.Singleton);
            
            builder.Register<GameplayStateMachine>(Lifetime.Singleton);
            builder.Register<StartGameState>(Lifetime.Singleton);
            builder.Register<WaveStartState>(Lifetime.Singleton);
            builder.Register<WaveVictoryState>(Lifetime.Singleton);
            builder.Register<WaveFailState>(Lifetime.Singleton);
            builder.Register<WaitForNextWaveState>(Lifetime.Singleton);
            builder.Register<PlayerDeathState>(Lifetime.Singleton);
            builder.Register<RestartGameState>(Lifetime.Singleton);
        }
    }
}