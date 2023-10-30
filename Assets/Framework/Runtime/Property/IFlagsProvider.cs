namespace GBG.Framework.Property
{
    public interface IFlagsProvider
    {
        ulong GetFlags();
        bool ContainsFlagAt(int flagIndex);
        bool ContainsFlags(ulong flags);
    }
}
