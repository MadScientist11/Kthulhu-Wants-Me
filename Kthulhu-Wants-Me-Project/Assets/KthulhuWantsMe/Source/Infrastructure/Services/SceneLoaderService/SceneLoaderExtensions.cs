using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService
{
    public static class SceneLoaderExtensions
    {
        public static async UniTask LoadSceneInjected(this ISceneLoader sceneLoader,
            string path, LoadSceneMode loadSceneMode,
            LifetimeScope sceneParentScope, IInstaller sceneInstaller)
        {
            using (LifetimeScope.EnqueueParent(sceneParentScope))
            {
                using (LifetimeScope.Enqueue(sceneInstaller))
                {
                    await sceneLoader.LoadScene(path, loadSceneMode);
                }
            }
        }
    }
}