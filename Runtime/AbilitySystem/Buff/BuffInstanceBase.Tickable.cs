using GBG.GameToolkit.Logic;

namespace GBG.GameToolkit.Ability.Buff
{
    partial class BuffInstanceBase : ITickable
    {
        int ITickable.TickChannel { get; }
        int ITickable.TickPriority { get; }
        protected IClock Clock { get; }


        void ITickable.Tick()
        {
            if (Target == null)
            {
                return;
            }

            if (_isExpiredNonVirtual)
            {
                return;
            }

            OnTick();
        }

        void ITickable.LateTick()
        {
            if (Target == null)
            {
                return;
            }

            if (_isExpiredNonVirtual)
            {
                return;
            }

            ElapsedTime += Clock.DeltaTime;
            OnLateTick();

            //_isExpiredNonVirtual = IsExpired();
        }

        protected virtual void OnTick() { }
        protected virtual void OnLateTick() { }
    }
}