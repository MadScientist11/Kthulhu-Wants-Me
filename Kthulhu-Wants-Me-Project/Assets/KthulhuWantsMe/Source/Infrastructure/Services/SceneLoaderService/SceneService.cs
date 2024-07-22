using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService
{
    public enum SceneId
    {
        StartUp = 0,
        MainMenu = 1,
        Game = 2,
        UI = 3,
    }

    public interface ISceneService
    {
        UniTask LoadScene(SceneId sceneId, LoadSceneMode loadSceneMode);
        UniTask UnloadSceneAsync(SceneId sceneId);
    }

    public class SceneService : ISceneService
    {
        public async UniTask LoadScene(SceneId sceneId, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync((int)sceneId, loadSceneMode);
        }

        public async UniTask UnloadSceneAsync(SceneId sceneId)
        {
            await SceneManager.UnloadSceneAsync((int)sceneId);
        }
    }
}