using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService
{
    public interface ISceneService
    {
        UniTask LoadScene(string path, LoadSceneMode loadSceneMode);
        UniTask UnloadSceneAsync(string path);
    }

    public class SceneService : ISceneService
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