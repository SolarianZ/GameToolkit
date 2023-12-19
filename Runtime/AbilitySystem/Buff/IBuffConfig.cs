namespace GBG.GameToolkit.Ability.Buff
{
    public interface IBuffConfig
    {
        IRuntimeBuffConfig GetOrCreateRuntimeConfig();
    }
}