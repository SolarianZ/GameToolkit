namespace GBG.GameToolkit.Logic
{
    public interface ITickable
    {
        int TickChannel { get; }
        int TickPriority { get; }

        void Tick();
        void LateTick();
    }
}
