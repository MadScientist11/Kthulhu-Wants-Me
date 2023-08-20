using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IResourceManager : IInitializableService
    {
        UniTask<T> ProvideAsset<T>(string path) where T : Object;
    }

    public class ResourcesManager : IResourceManager
    {
        public ResourcesManager()
        {
            Debug.Log("Resource");
        }
        public async UniTask Initialize()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask<T> ProvideAsset<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(path) as T;
        }
    }
}
