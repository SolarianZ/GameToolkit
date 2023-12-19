using GBG.GameToolkit.Logic;
using GBG.GameToolkit.Property;
using System;

namespace GBG.GameToolkit.Ability.Buff
{
    //public enum BuffState
    //{
    //}

    public interface IBuffInstance
    {
        int InstanceId { get; }
        IRuntimeBuffConfig Config { get; }

        void OnAttachToTarget(object target);
        void OnDetachFromTarget();

        bool ProvideProperties();
        bool ProvideFlags();
    }
}