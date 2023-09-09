﻿using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Spell;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IDataProvider : IInitializableService
    {
        PlayerConfiguration PlayerConfig { get; }
        TentacleConfiguration TentacleConfig { get; }
        CyaeghaConfiguration CyaeghaConfig { get; }
        YithConfiguration YithConfig { get; }
        PortalConfiguration PortalConfig { get; }
        RandomBuffsContainer BuffsContainer { get; }
    }
    
    public class DataProvider : IDataProvider
    {
        public bool IsInitialized { get; set; }
        private const string PlayerConfigurationPath = "PlayerConfiguration";
        private const string TentacleConfigurationPath = "TentacleConfiguration";
        private const string PortalConfigurationPath = "PortalConfiguration";
        private const string CyaeghaConfigurationPath = "CyaeghaConfiguration";
        private const string YithConfigurationPath = "YithConfiguration";
        private const string BuffsContainerPath = "BuffsContainer";
        public PlayerConfiguration PlayerConfig { get; private set; }
        public TentacleConfiguration TentacleConfig { get; private set; }
        
        public CyaeghaConfiguration CyaeghaConfig { get; private set; }
        public YithConfiguration YithConfig { get; private set; }
        public PortalConfiguration PortalConfig { get; private set; }
        public RandomBuffsContainer BuffsContainer { get; private set; }


        public async UniTask Initialize()
        {
            IsInitialized = true;
            PlayerConfig = (PlayerConfiguration)await Resources.LoadAsync<PlayerConfiguration>(PlayerConfigurationPath);
            TentacleConfig = (TentacleConfiguration)await Resources.LoadAsync<TentacleConfiguration>(TentacleConfigurationPath);
            PortalConfig = (PortalConfiguration)await Resources.LoadAsync<PortalConfiguration>(PortalConfigurationPath);
            CyaeghaConfig = (CyaeghaConfiguration)await Resources.LoadAsync<CyaeghaConfiguration>(CyaeghaConfigurationPath);
            YithConfig = (YithConfiguration)await Resources.LoadAsync<YithConfiguration>(YithConfigurationPath);
            BuffsContainer = (RandomBuffsContainer)await Resources.LoadAsync<RandomBuffsContainer>(BuffsContainerPath);
        }
    }
}