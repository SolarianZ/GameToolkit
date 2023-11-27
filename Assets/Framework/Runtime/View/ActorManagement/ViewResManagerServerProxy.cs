namespace GBG.Framework.View.ActorManagement
{
    public sealed class ViewResServerProxy : IRecyclableViewRes
    {
        public object ResKey { get; set; }


        public void OnRecycleRes() { }

        public void OnUseRes() { }

        public void OnDestroyRes() => ResKey = null;
    }

    public sealed class ViewResManagerServerProxy : IViewResManager
    {
        public bool IsLoadingAnyReses => false;


        public bool IsResLoaded(object resKey) => true;

        public bool IsResLoading(object resKey) => false;

        public void LoadRes(object resKey) { }

        public void UnloadRes(object resKey) { }

        public bool TryGetLoadedRes(object resKey, out object res)
        {
            res = null;
            return false;
        }
    }
}
