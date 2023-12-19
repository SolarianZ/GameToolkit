namespace GBG.GameToolkit.Event
{
    public interface IEventHandler { }

    public interface IEventHandler<TEventArgs> : IEventHandler
    {
        void HandleEvent(object sender, TEventArgs args);
    }
}
