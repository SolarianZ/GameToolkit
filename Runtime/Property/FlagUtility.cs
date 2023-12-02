namespace GBG.GameToolkit.Property
{
    public static class FlagUtility
    {
        public static bool ContainsFlagAt(this ulong flags, int flagIndex)
        {
            return flags.ContainsFlags(1UL << flagIndex);
        }

        public static bool ContainsFlags(this ulong a, ulong b)
        {
            return (a & b) == b;
        }

        public static ulong AddFlagAt(this ulong flags, int flagIndex)
        {
            return flags.AddFlags(1UL << flagIndex);
        }

        public static ulong RemoveFlagAt(this ulong flags, int flagIndex)
        {
            return flags.RemoveFlags(1UL << flagIndex);
        }

        public static ulong AddFlags(this ulong a, ulong b)
        {
            return a | b;
        }

        public static ulong RemoveFlags(this ulong a, ulong b)
        {
            return a & (~b);
        }
    }
}
