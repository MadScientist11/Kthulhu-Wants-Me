using KthulhuWantsMe.Source.Infrastructure.EntryPoints;
using KthulhuWantsMe.Source.Infrastructure.Installers;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    public class GameLifetimeScope : DefaultInjectionScope
    {
        [SerializeField] private SceneDataProvider _sceneDataProvider;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Install(new GameInstaller(_sceneDataProvider));
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}