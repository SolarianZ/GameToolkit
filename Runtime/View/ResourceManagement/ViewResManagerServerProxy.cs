namespace GBG.GameToolkit.View.ResourceManagement
{
    public sealed class ViewResManagerServerProxy : IViewResManager
    {
        #region Res

        public bool IsLoadingAnyReses() => false;
        public bool IsResLoaded(object resKey) => true;
        public bool IsResLoading(object resKey) => false;
        public void LoadRes(object resKey) { }
        public void UnloadRes(object resKey) { }
        public bool TryGetLoadedRes(object resKey, out object res)
        {
            res = null;
            return false;
        }

        #endregion


        #region Scene

        public bool IsLoadingAnyScenes() => false;
        public bool IsSceneLoaded(object sceneKey) => true;
        public bool IsSceneLoading(object sceneKey) => false;
        public void LoadScene(object sceneKey) { }
        public void UnloadScene(object sceneKey) { }

        #endregion
    }
}
