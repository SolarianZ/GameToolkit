namespace GBG.Framework.View.ResourceManagement
{
    public sealed class RecyclableViewResServerProxy : IRecyclableViewRes
    {
        public object ResKey { get; set; }

        public void OnUseRes() { }
        public void OnRecycleRes() { }
        public void OnDestroyRes() => ResKey = null;
    }
}
