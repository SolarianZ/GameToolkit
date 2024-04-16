using GBG.GameToolkit.ConfigData;
using GBG.GameToolkit.Property;
using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Ability.Buff
{
    public partial class DefaultBuffManager<TBuffConfig> : IBuffManager
        where TBuffConfig : IBuffConfig
    {
        private readonly IBuffFactory _buffFactory;
        private readonly IConfigTable _configsProvider;
        private object _buffTarget;
        private object _buffContext;
        private bool _isInitialized;
        private readonly List<IBuffInstance> _activeBuffs = new();


        public DefaultBuffManager(IBuffFactory buffFactory, IConfigTable config,
            IPropertySpecsProvider propertySpecsProvider)
        {
            _buffFactory = buffFactory;
            _configsProvider = config;
            _propertiesContainer = new DefaultPropertiesContainer(propertySpecsProvider, true);
            _flagsContainer = new DefaultFlagsContainer(true);
        }

        public void Initialize(object buffTarget, object buffContext)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("​The buff manager has already been initialized.");
            }

            _buffTarget = buffTarget;
            _buffContext = buffContext;
        }

        public void Deinitialize()
        {
            if (!_isInitialized)
            {
                return;
            }

            _buffTarget = null;
            _buffContext = null;

            // Detach all buffs
            for (int i = 0; i < _activeBuffs.Count; i++)
            {
                IBuffInstance buffInstance = _activeBuffs[i];
                buffInstance.OnDetachFromTarget();
            }
            _activeBuffs.Clear();

            ClearAllPropertiesProviders();
            ClearAllFlagsProviders();
        }

        public int AttachBuffToTarget(int buffConfigId)
        {
            TBuffConfig buffConfig = _configsProvider.GetConfig<TBuffConfig>(buffConfigId);
            if (buffConfig == null)
            {
                throw new ArgumentException($"Cannot load buff config with id '{buffConfigId}'.",
                    nameof(buffConfigId));
            }

            IRuntimeBuffConfig runtimeBuffConfig = buffConfig.GetOrCreateRuntimeConfig();
            IBuffInstance buffInstance;
            switch (runtimeBuffConfig.OverlapMode)
            {
                case BuffOverlapMode.Overlap:
                {
                    int overlapCount = GetBuffOverlapCount(buffConfigId);
                    if (runtimeBuffConfig.MaxOverlapCount <= 0 ||
                        overlapCount < runtimeBuffConfig.MaxOverlapCount)
                    {
                        buffInstance = _buffFactory.AllocBuffInstance(buffConfigId);
                    }
                    else
                    {
                        return 0;
                    }

                    break;
                }

                case BuffOverlapMode.Ignore:
                {
                    int overlapCount = GetBuffOverlapCount(buffConfigId);
                    if (overlapCount > 0)
                    {
                        return 0;
                    }

                    buffInstance = _buffFactory.AllocBuffInstance(buffConfigId);
                    break;
                }

                default:
                    throw new Exception($"Unknown buff overlap mode: {runtimeBuffConfig.OverlapMode}.");
            }

            _activeBuffs.Add(buffInstance);
            buffInstance.OnAttachToTarget(_buffTarget, _buffContext);

            if (buffInstance.ProvideProperties() && buffInstance is IPropertiesProvider propertiesProvider)
            {
                AddPropertiesProvider(propertiesProvider);
            }

            if (buffInstance.ProvideFlags() && buffInstance is IFlagsProvider flagsProvider)
            {
                AddFlagsProvider(flagsProvider);
            }

            return buffInstance.InstanceId;
        }

        public bool DetachBuffFromTarget(int buffInstanceId)
        {
            for (int i = 0; i < _activeBuffs.Count; i++)
            {
                IBuffInstance buffInstance = _activeBuffs[i];
                if (buffInstance.InstanceId != buffInstanceId)
                {
                    continue;
                }

                _activeBuffs.RemoveAt(i);

                if (buffInstance.ProvideProperties() && buffInstance is IPropertiesProvider propertiesProvider)
                {
                    RemovePropertiesProvider(propertiesProvider);
                }

                if (buffInstance.ProvideFlags() && buffInstance is IFlagsProvider flagsProvider)
                {
                    RemoveFlagsProvider(flagsProvider);
                }

                buffInstance.OnDetachFromTarget();

                return true;
            }

            return false;
        }

        public int GetBuffOverlapCount(int buffConfigId)
        {
            int overlapCount = 0;
            foreach (IBuffInstance buffInstance in _activeBuffs)
            {
                if (buffInstance.Config.Id == buffConfigId && !buffInstance.IsExpired())
                {
                    overlapCount++;
                }
            }

            return overlapCount;
        }
    }
}