namespace GBG.GameToolkit.Property
{
    public interface IFlagsContainer : IFlagsProvider
    {
        bool AddFlagsProvider(IFlagsProvider flagsProvider);
        bool RemoveFlagsProvider(IFlagsProvider flagsProvider);
    }
}
