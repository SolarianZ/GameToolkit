namespace GBG.Framework.View.ActorManagement
{
    public interface IRecyclableViewRes
    {
        object ResKey { get; }

        void OnUseRes();
        void OnRecycleRes();
        void OnDestroyRes();
    }
}
