﻿using Cysharp.Threading.Tasks;
using SimpleAudioManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.Audio
{
    public interface IBackgroundMusicPlayer
    {
        void PlayBattleMusic();
        void PlayDefeatMusic();
        void PlayConcernMusic();
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
            Object.DontDestroyOnLoad(_simpleAudioManager);
        }

        public void PlayBattleMusic() => 
            _simpleAudioManager.PlaySong(0);

        public void PlayDefeatMusic() => 
            _simpleAudioManager.PlaySong(2);

        public void PlayConcernMusic() => 
            _simpleAudioManager.PlaySong(1);
    }
}