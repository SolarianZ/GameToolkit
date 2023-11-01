namespace GBG.Framework.View.ActorManagement
{
    public interface IViewActor
    {
        int ActorId { get; }

        void OnUse();
        void OnRecycle();
    }
}
