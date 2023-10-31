namespace GBG.Framework.Logic
{
    public interface IClock
    {
        ulong FrameId { get; }
        ulong Time { get; }
        ulong DeltaTime { get; }
    }
}
