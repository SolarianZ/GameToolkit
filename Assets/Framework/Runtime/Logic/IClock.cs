namespace GBG.GameToolkit.Logic
{
    public interface IClock
    {
        ulong FrameId { get; }
        ulong Time { get; }
        uint DeltaTime { get; }
    }
}
