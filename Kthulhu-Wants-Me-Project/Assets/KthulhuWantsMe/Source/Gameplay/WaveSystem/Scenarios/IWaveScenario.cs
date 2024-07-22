using System;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Scenarios
{
    public interface IWaveScenario
    {
        event Action BatchCleared;
        void Initialize();
        void Dispose();
    }
}