namespace GBG.GameToolkit.Ability.Buff
{
    public interface IBuffFactory
    {
        IBuffInstance AllocBuffInstance(int buffConfigId);
        void RecycleBuffInstance(IBuffInstance buffInstance);
    }
}