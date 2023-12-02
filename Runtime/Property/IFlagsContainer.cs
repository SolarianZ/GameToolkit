namespace GBG.GameToolkit.Property
{
    public interface IFlagsContainer : IFlagsProvider
    {
        void AddFlagAt(int flagIndex);
        void RemoveFlagAt(int flagIndex);
        void AddFlags(ulong flags);
        void RemoveFlags(ulong flags);
    }
}
