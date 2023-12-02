namespace GBG.GameToolkit.Logic
{
    public interface ITickable
    {
        int Channel { get; }
        int Priority { get; }

        void Tick();
        void LateTick();
    }
}
