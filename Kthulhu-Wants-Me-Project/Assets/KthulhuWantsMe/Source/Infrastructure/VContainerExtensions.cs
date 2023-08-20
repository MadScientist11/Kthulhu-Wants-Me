using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure
{
    public static class VContainerExtensions
    {
        public static void Install(this IContainerBuilder builder, IInstaller installer) => 
            installer.Install(builder);
        
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