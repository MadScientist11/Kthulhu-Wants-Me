using Cysharp.Threading.Tasks;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IPreInitializableService
    {
        UniTask Initialize();
    }
}