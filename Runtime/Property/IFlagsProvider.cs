using System;

namespace GBG.GameToolkit.Property
{
    //public delegate void FlagsChangedHandler();

    public interface IFlagsProvider
    {
        event Action FlagsChanged;


        ulong GetFlags();

        bool ContainsFlagAt(int targetFlagIndex)
        {
            ulong flags = GetFlags();
            ulong targetFlag = 1UL << targetFlagIndex;
            return (flags & targetFlag) != 0;
        }

        bool ContainsFlagsAll(ulong targetFlags)
        {
            ulong flags = GetFlags();
            return (flags & targetFlags) == targetFlags;
        }

        bool ContainsFlagsAny(ulong targetFlags)
        {
            ulong flags = GetFlags();
            return (flags & targetFlags) != 0;
        }
    }
}
