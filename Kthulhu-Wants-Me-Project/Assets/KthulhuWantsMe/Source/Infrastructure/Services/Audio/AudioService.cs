using System.Threading;
using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.Audio
{
    public interface IAudioService
    {
    }

    public class AudioService : IAudioService, IInitializableService
    {
        public bool IsInitialized { get; set; }
        
        private MMSoundManager _sfxPlayer;
        
        private readonly IResourceManager _resourceManager;
        private readonly IBackgroundMusicPlayer _backgroundMusicPlayer;
        private readonly IObjectResolver _instantiator;

        public AudioService(IResourceManager resourceManager, IBackgroundMusicPlayer backgroundMusicPlayer, IObjectResolver instantiator)
        {
            _instantiator = instantiator;
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _resourceManager = resourceManager;
        }

        public async UniTask Initialize()
        {
            MMSoundManager _sfxPlayerPrefab =  await _resourceManager.ProvideAssetAsync<MMSoundManager>("AudioPlayer");
            _sfxPlayer = _instantiator.Instantiate(_sfxPlayerPrefab);
        }
    }
}