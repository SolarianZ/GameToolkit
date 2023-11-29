namespace GBG.Framework.View.ResourceManagement
{
    public interface IViewResManager
    {
        bool IsLoadingAnyReses();
        bool IsResLoaded(object resKey);
        bool IsResLoading(object resKey);
        void LoadRes(object resKey);
        void UnloadRes(object resKey);
        bool TryGetLoadedRes(object resKey, out object res);


        #region Scene

        bool IsLoadingAnyScenes();
        bool IsSceneLoaded(object sceneKey);
        bool IsSceneLoading(object sceneKey);
        void LoadScene(object sceneKey);
        void UnloadScene(object sceneKey);

        #endregion
    }
}
