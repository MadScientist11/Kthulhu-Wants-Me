using System;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public interface IWaveScenario
    {
        event Action BatchCleared;
        void Initialize();
        void Dispose();
    }
}