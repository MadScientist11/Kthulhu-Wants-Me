using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ISceneLoader
    {
        UniTask LoadScene(string path, LoadSceneMode loadSceneMode);
    }

    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadScene(string path, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(path, loadSceneMode);
        }
    }
}