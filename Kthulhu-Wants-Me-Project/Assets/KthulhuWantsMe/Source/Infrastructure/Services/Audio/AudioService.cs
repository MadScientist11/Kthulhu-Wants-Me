using Cysharp.Threading.Tasks;
using Freya;
using MoreMountains.Tools;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.Audio
{
    public interface IAudioService
    {
        void SetVolume(string key, int value);
    }

    public class AudioService : IAudioService, IInitializableService
    {
        public bool IsInitialized { get; set; }

        private AudioMixer _mixer;
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
            _mixer =  await _resourceManager.ProvideAssetAsync<AudioMixer>("GameAudioMixer");
            _sfxPlayer = _instantiator.Instantiate(_sfxPlayerPrefab);
        }

        public void SetVolume(string key, int value)
        {
            float remapped = Mathfs.Remap(0, 100, -80, 0, value);
            _mixer.SetFloat(key, remapped);
        }
    }
}