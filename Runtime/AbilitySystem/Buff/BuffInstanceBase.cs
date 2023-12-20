using GBG.GameToolkit.Property;
using System;

namespace GBG.GameToolkit.Ability.Buff
{
    public abstract partial class BuffInstanceBase : IBuffInstance
    {
        public abstract int InstanceId { get; }
        public abstract IRuntimeBuffConfig Config { get; }
        public ulong ElapsedTime { get; private set; }
        protected object Target { get; private set; }
        protected object Context { get; private set; }

        private bool _isExpiredNonVirtual;


        protected BuffInstanceBase(IPropertySpecsProvider propertySpecsProvider)
        {
            _propertySpecsProvider = propertySpecsProvider;
        }

        public virtual ulong GetBuffDuration()
        {
            return Config.Duration;
        }

        public virtual int GetBuffAvailableTimes()
        {
            return Config.AvailableTimes;
        }

        public virtual int GetBuffUsedTimes()
        {
            return 0;
        }

        public virtual bool IsExpired()
        {
            if (_isExpiredNonVirtual)
            {
                return true;
            }

            ulong buffDuration = GetBuffDuration();
            if (buffDuration > 0 && ElapsedTime >= buffDuration)
            {
                return true;
            }

            int availableTimes = GetBuffAvailableTimes();
            if (availableTimes > 0)
            {
                int usedTimes = GetBuffUsedTimes();
                if (usedTimes >= availableTimes)
                {
                    return true;
                }
            }

            return false;
        }


        void IBuffInstance.OnAttachToTarget(object target, object context)
        {
            if (Target != null)
            {
                throw new InvalidOperationException(
                    $"The buff has already been attached to '{Target}'." +
                    " You cannot attach it again.");
            }

            Target = target;
            Context = context;
            _isExpiredNonVirtual = false;

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
            Context = null;
            _isExpiredNonVirtual = true;
        }

        protected virtual void OnAttachToTarget() { }
        protected virtual void OnDetachFromTarget() { }
    }
}