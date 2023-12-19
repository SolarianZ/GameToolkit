using GBG.GameToolkit.Logic;

namespace GBG.GameToolkit.Ability.Buff
{
    partial class BuffInstanceBase :ITickable
    {
        int ITickable.Channel { get; }
        int ITickable.Priority { get; }


        void ITickable.Tick()
        {
            if (Target == null)
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

            OnLateTick();
        }

        protected virtual void OnTick() { }
        protected virtual void OnLateTick() { }
    }
}