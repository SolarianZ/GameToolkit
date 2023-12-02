namespace GBG.GameToolkit.View.ResourceManagement
{
    public interface IRecyclableViewRes
    {
        object ResKey { get; }

        void OnUseRes();
        void OnRecycleRes();
        void OnDestroyRes();
    }
}
