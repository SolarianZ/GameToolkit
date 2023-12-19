using GBG.GameToolkit.ConfigData;
using System.Collections.Generic;

namespace GBG.GameToolkit.Ability.Buff
{
    public interface IRuntimeBuffConfig
    {
        int Id { get; }
        BuffOverlapMode OverlapMode { get; }
        int MaxOverlapCount { get; }
        ulong Duration { get; }
        int AvailableTimes { get; }
        ulong Flags { get; }
        IReadOnlyList<Property.Property> Properties { get; }
        IReadOnlyList<CustomParamPair> CustomParams { get; }

        string GetCustomParam(string key, string defaultValue);
        bool TryGetCustomParam(string key, out string value);
    }
}