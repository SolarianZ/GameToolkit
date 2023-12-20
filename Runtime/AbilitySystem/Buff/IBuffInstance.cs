using GBG.GameToolkit.Logic;

namespace GBG.GameToolkit.Ability.Buff
{
    //public enum BuffState
    //{
    //}

    public interface IBuffInstance : ITickable
    {
        int InstanceId { get; }
        IRuntimeBuffConfig Config { get; }
        ulong ElapsedTime { get; }

        void OnAttachToTarget(object target, object context);
        void OnDetachFromTarget();

        bool ProvideProperties();
        bool ProvideFlags();

        ulong GetBuffDuration();
        int GetBuffAvailableTimes();
        int GetBuffUsedTimes();
        bool IsExpired();
    }
}