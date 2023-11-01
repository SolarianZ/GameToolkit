using System.Collections.Generic;

namespace GBG.Framework.View.SceneManagement
{
    public sealed class ViewSceneManagerServerProxy : IViewSceneManager
    {
        public IReadOnlyList<int> LoadedScenes => _loadedScenes;
        public IReadOnlyList<int> LoadingScenes => _loadingScenes;

        private readonly int[] _loadedScenes = new int[0];
        private readonly int[] _loadingScenes = new int[0];


        public bool IsSceneLoaded(int SceneId) => true;

        public void LoadScene(int SceneId) { }

        public void UnloadScene(int SceneId) { }
    }
}
