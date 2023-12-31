﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public class ProgressData
    {
        public int CompletedWaveIndex;
        public Dictionary<Guid, int> CompletedSkillBranchStages;
    }
    public interface IProgressService
    {
        ProgressData ProgressData { get; }
        void Reset();
    }

    public class ProgressService : IProgressService, IInitializableService
    {
        public ProgressData ProgressData { get; } = new();

        public bool IsInitialized { get; set; }
        public UniTask Initialize()
        {
            IsInitialized = true;
            Reset();
            return UniTask.CompletedTask;
        }

        public void Reset()
        {
            ProgressData.CompletedWaveIndex = -1;
            ProgressData.CompletedSkillBranchStages = new();
        }
    }
}