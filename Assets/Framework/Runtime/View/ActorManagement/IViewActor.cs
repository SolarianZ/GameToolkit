namespace GBG.Framework.View.ActorManagement
{
    public interface IViewActor
    {
        object ResKey { get; }

        void OnUseActor();
        void OnRecycleActor();
        void OnDestroyActor();
    }
}
