using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IResourceManager : IInitializableService
    {
        UniTask<T> ProvideAssetAsync<T>(string path) where T : Object;
    }

    public class ResourcesManager : IResourceManager
    {
        public bool IsInitialized { get; set; }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            await UniTask.CompletedTask;
        }

        public async UniTask<T> ProvideAssetAsync<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(path) as T;
        }
        
        public T[] ProvideAssets<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }
    }
}
