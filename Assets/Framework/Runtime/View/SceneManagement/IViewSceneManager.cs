using System.Collections.Generic;
using System.Linq;

namespace GBG.Framework.View.SceneManagement
{
    public interface IViewSceneManager
    {
        IReadOnlyList<int> LoadedScenes { get; }
        IReadOnlyList<int> LoadingScenes { get; }
        bool IsLoadingAnyScenes => LoadingScenes != null && LoadingScenes.Count > 0;

        bool IsSceneLoaded(int sceneId) => LoadedScenes?.Contains(sceneId) ?? false;
        bool IsSceneLoading(int sceneId) => LoadingScenes?.Contains(sceneId) ?? false;
        void LoadScene(int sceneId);
        void UnloadScene(int sceneId);
    }
}
