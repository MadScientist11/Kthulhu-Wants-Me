using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IInitializableService
    {
        bool IsInitialized { get; set; }
        UniTask Initialize();
    }
}