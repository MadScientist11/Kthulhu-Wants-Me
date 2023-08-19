using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IInitializableService
    {
        UniTask Initialize();
    }
}