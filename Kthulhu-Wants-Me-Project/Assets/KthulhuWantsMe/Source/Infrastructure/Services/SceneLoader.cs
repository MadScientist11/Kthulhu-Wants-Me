using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface ISceneLoader
    {
        UniTask LoadScene(string path, LoadSceneMode loadSceneMode);
    }

    public class SceneLoader : ISceneLoader
    {
        private readonly AppLifetimeScope _appLifetimeScope;

        public SceneLoader(AppLifetimeScope appLifetimeScope)
        {
            _appLifetimeScope = appLifetimeScope;
        }

        public async UniTask LoadScene(string path, LoadSceneMode loadSceneMode)
        {
            using (LifetimeScope.EnqueueParent(_appLifetimeScope))
            {
                await SceneManager.LoadSceneAsync(path, loadSceneMode);
            }
        }
    }
}