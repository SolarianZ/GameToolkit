using GBG.GameToolkit.Property;
using System;

namespace GBG.GameToolkit.Ability.Buff
{
    public abstract partial class BuffInstanceBase : IBuffInstance
    {
        public abstract int InstanceId { get; }
        public abstract IRuntimeBuffConfig Config { get; }
        protected object Target { get; private set; }

        // TODO: Tick duration and used times

        protected BuffInstanceBase(IPropertySpecsProvider propertySpecsProvider)
        {
            _propertySpecsProvider = propertySpecsProvider;
        }

        protected virtual ulong GetBuffDuration()
        {
            return Config.Duration;
        }

        protected virtual int GetBuffUsedTimes()
        {
            return 0;
        }


        void IBuffInstance.OnAttachToTarget(object target)
        {
            if (Target != null)
            {
                throw new InvalidOperationException(
                    $"The buff has already been attached to '{Target}'." +
                    " You cannot attach it again.");
            }

            Target = target;

            OnAttachToTarget();
        }

        void IBuffInstance.OnDetachFromTarget()
        {
            if (Target == null)
            {
                throw new InvalidOperationException(
                    "The buff has already been detached from its target." +
                    " You cannot detach it again.");
            }

            OnDetachFromTarget();

            Target = null;
        }

        protected virtual void OnAttachToTarget() { }
        protected virtual void OnDetachFromTarget() { }
    }
}