using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IResourceManager
    {
        UniTask<T> ProvideAsset<T>(string path) where T : Object;
    }

    public class ResourcesManager : IResourceManager
    {
        public async UniTask<T> ProvideAsset<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(path) as T;
        } 
    }
}
