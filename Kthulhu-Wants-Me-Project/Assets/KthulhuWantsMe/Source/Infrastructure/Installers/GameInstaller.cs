using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEditor.Compilation;
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
                .RegisterComponent(_sceneDataProvider)
                .AsImplementedInterfaces();


            builder.Register<ThePlayer>(Lifetime.Singleton).As<IInitializable>().AsSelf();
            
            builder
                .Register<LootService>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            

            builder
                .Register<PortalSystem>(Lifetime.Scoped)
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
                .Register<EnemyStatsProvider>(Lifetime.Scoped)
                .AsSelf();
            
            builder
                .Register<EnemyStatsScalingService>(Lifetime.Scoped)
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
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<StartGameState>(Lifetime.Singleton);
            builder.Register<WaveStartState>(Lifetime.Singleton);
            builder.Register<WaveCompleteState>(Lifetime.Singleton);
        }
    }
}