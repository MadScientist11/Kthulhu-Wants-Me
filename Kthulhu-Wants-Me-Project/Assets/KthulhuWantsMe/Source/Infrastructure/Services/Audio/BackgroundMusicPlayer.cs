using Cysharp.Threading.Tasks;
using SimpleAudioManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.Audio
{
    public interface IBackgroundMusicPlayer
    {
        void Play();
    }

    public class BackgroundMusicPlayer : IBackgroundMusicPlayer, IInitializableService
    {
        public bool IsInitialized { get; set; }
        
        private const string BgmPlayerPath = "BackgroundMusicPlayer";

        private Manager _simpleAudioManager;
        private readonly IResourceManager _resourceManager;
        private readonly IObjectResolver _instantiator;

        public BackgroundMusicPlayer(IResourceManager resourceManager, IObjectResolver instantiator)
        {
            _instantiator = instantiator;
            _resourceManager = resourceManager;
        }

        public async UniTask Initialize()
        {
            Manager bgmPlayerPrefab = await _resourceManager.ProvideAssetAsync<Manager>(BgmPlayerPath);
            _simpleAudioManager = _instantiator.Instantiate(bgmPlayerPrefab);
        }

        public void Play()
        {
            _simpleAudioManager.PlaySong(0);
        }
    }
}