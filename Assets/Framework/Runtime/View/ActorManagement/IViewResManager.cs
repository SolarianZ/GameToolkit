namespace GBG.Framework.View.ActorManagement
{
    public interface IViewResManager
    {
        bool IsLoadingAnyReses { get; }

        bool IsResLoaded(object resKey);
        bool IsResLoading(object resKey);
        void LoadRes(object resKey);
        void UnloadRes(object resKey);

        bool TryGetLoadedRes(object resKey, out object res);
    }
}
