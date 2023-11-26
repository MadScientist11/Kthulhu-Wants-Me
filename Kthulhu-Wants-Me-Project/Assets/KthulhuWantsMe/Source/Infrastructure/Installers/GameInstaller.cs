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
                .Register<GameFactory>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemiesAIBrainService>(Lifetime.Scoped)
                .AsImplementedInterfaces();


            builder
                .RegisterComponentOnNewGameObject<InGameConsoleService>(Lifetime.Singleton, "InGameConsoleService")
                .AsSelf();

            builder
                .RegisterComponent(_sceneDataProvider)
                .AsImplementedInterfaces();


            builder.Register<ThePlayer>(Lifetime.Scoped)
                .As<IInitializable>()
                .As<ITickable>()
                .AsSelf();
            
            builder
                .Register<LootService>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<RoomOverseer>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            

            
            builder
                .Register<UpgradeService>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            

            builder
                .Register<PortalFactory>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<BuffDebuffService>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<BuffDebuffFactory>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemyStatsScalingService>(Lifetime.Scoped)
                .AsSelf();
            
            builder
                .Register<GameApp>(Lifetime.Scoped)
                .AsSelf()
                .AsImplementedInterfaces();
            
            builder
                .Register<EnemyStatsProvider>(Lifetime.Scoped)
                .AsSelf();
            
            builder
                .Register<WaveSystemDirector>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder
                .Register<ProjectileArcFactory>(Lifetime.Scoped)
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