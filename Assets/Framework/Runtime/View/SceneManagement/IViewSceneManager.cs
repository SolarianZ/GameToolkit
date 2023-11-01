using System.Collections.Generic;
using System.Linq;

namespace GBG.Framework.View.SceneManagement
{
    public interface IViewSceneManager
    {
        IReadOnlyList<int> LoadedScenes { get; }
        IReadOnlyList<int> LoadingScenes { get; }
        bool IsLoadingAnyScenes => LoadingScenes != null && LoadingScenes.Count > 0;

        bool IsSceneLoaded(int SceneId) => LoadedScenes?.Contains(SceneId) ?? false;
        bool IsSceneLoading(int SceneId) => LoadingScenes?.Contains(SceneId) ?? false;
        void LoadScene(int SceneId);
        void UnloadScene(int SceneId);
    }
}
