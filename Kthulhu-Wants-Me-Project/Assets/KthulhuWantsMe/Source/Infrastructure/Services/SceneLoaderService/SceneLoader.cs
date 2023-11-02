using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService
{
    public interface ISceneLoader
    {
        UniTask LoadScene(string path, LoadSceneMode loadSceneMode);
        UniTask UnloadSceneAsync(string path);
    }

    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadScene(string path, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(path, loadSceneMode);
        }

        public async UniTask UnloadSceneAsync(string path)
        {
            await SceneManager.UnloadSceneAsync(path);
        }
    }
}