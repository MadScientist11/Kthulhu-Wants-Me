using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public class ProgressData
    {
        public int CompletedWaveIndex;
    }
    public interface IProgressService
    {
        ProgressData ProgressData { get; }
    }

    public class ProgressService : IProgressService, IInitializableService
    {
        public ProgressData ProgressData { get; } = new();

        public bool IsInitialized { get; set; }
        public UniTask Initialize()
        {
            IsInitialized = true;
            ProgressData.CompletedWaveIndex = -1;
            return UniTask.CompletedTask;
        }
    }
}