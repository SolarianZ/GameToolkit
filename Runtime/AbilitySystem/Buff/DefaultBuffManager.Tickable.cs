using GBG.GameToolkit.Logic;
using GBG.GameToolkit.Property;
using System.Collections.Generic;

namespace GBG.GameToolkit.Ability.Buff
{
    public partial class DefaultBuffManager<TBuffConfig> : ITickable
    {
        int ITickable.TickChannel { get; }
        int ITickable.TickPriority { get; }

        private readonly List<int> _expiredBuffIndices = new();


        void ITickable.Tick()
        {
            for (int i = 0; i < _activeBuffs.Count; i++)
            {
                IBuffInstance buffInstance = _activeBuffs[i];
                if (!buffInstance.IsExpired())
                {
                    buffInstance.Tick();
                }
            }
        }

        void ITickable.LateTick()
        {
            _expiredBuffIndices.Clear(); // In case exceptions happened during detach
            for (int i = 0; i < _activeBuffs.Count; i++)
            {
                IBuffInstance buffInstance = _activeBuffs[i];
                if (buffInstance.IsExpired())
                {
                    _expiredBuffIndices.Add(i);
                }
                else
                {
                    buffInstance.LateTick();
                }
            }

            for (int i = _expiredBuffIndices.Count - 1; i >= 0; i--)
            {
                int buffIndex = _expiredBuffIndices[i];
                DetachExpiredBuffFromTargetByIndex(buffIndex);
            }
            _expiredBuffIndices.Clear();
        }

        private void DetachExpiredBuffFromTargetByIndex(int buffIndex)
        {
            IBuffInstance buffInstance = _activeBuffs[buffIndex];
            _activeBuffs.RemoveAt(buffIndex);

            if (buffInstance.ProvideProperties() && buffInstance is IPropertiesProvider propertiesProvider)
            {
                RemovePropertiesProvider(propertiesProvider);
            }

            if (buffInstance.ProvideFlags() && buffInstance is IFlagsProvider flagsProvider)
            {
                RemoveFlagsProvider(flagsProvider);
            }

            buffInstance.OnDetachFromTarget();
        }
    }
}