using GBG.GameToolkit.ConfigData;

namespace GBG.GameToolkit.Ability.Buff
{
    public interface IBuffConfig : IConfig
    {
        IRuntimeBuffConfig GetOrCreateRuntimeConfig();
    }
}