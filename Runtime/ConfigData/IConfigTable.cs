using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.ConfigData
{
    public interface IConfigTable
    {
        bool ContainsConfigList<T>() where T : IConfig;
        IConfigList<T> GetConfigList<T>() where T : IConfig;
        bool TryGetConfigList<T>(out IConfigList<T> configList) where T : IConfig;

        bool ContainsConfig<T>(int key) where T : IConfig;
        IReadOnlyList<T> GetConfigs<T>() where T : IConfig;
        T GetConfig<T>(int key) where T : IConfig;
        bool TryGetConfig<T>(int key, out T value) where T : IConfig;

        bool ContainsSingletonConfig<T>() where T : ISingletonConfig;
        T GetSingletonConfig<T>() where T : ISingletonConfig;
        bool TryGetSingletonConfig<T>(out T value) where T : ISingletonConfig;
    }

    public class DefaultConfigTable : IConfigTable
    {
        private const string LogTag = "GBG|DefaultConfigTable";
        private readonly Dictionary<Type, IConfigListPtr> _configTable;
        private readonly Dictionary<Type, ISingletonConfig> _singletonConfigTable;


        public DefaultConfigTable(IReadOnlyList<IConfigListPtr> configLists,
            IReadOnlyList<ISingletonConfig> singletonConfigs)
        {
            if (configLists == null)
            {
                _configTable = new Dictionary<Type, IConfigListPtr>(0);
            }
            else
            {
                _configTable = new(configLists.Count);
                foreach (IConfigListPtr configTable in configLists)
                {
                    _configTable.Add(configTable.GetConfigType(), configTable);
                }
            }

            if (singletonConfigs == null)
            {
                _singletonConfigTable = new Dictionary<Type, ISingletonConfig>(0);
            }
            else
            {
                _singletonConfigTable = new(singletonConfigs.Count);
                foreach (ISingletonConfig singletonConfig in singletonConfigs)
                {
                    _singletonConfigTable.Add(singletonConfig.GetConfigType(), singletonConfig);
                }
            }

            Debugger.LogInfo($"Constructed. Config list count: {_configTable.Count}, Singleton config count: {_singletonConfigTable.Count}.", this, LogTag);
        }

        public bool ContainsConfigList<T>() where T : IConfig
        {
            return _configTable.ContainsKey(typeof(T));
        }

        public IConfigList<T> GetConfigList<T>() where T : IConfig
        {
            if (TryGetConfigList<T>(out IConfigList<T> configList))
            {
                return configList;
            }

            return null;
        }

        public bool TryGetConfigList<T>(out IConfigList<T> configList) where T : IConfig
        {
            if (_configTable.TryGetValue(typeof(T), out IConfigListPtr listPtr))
            {
                configList = (IConfigList<T>)listPtr;
                return true;
            }

            configList = null;
            return false;
        }

        public bool ContainsConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfigList<T>(out IConfigList<T> configList))
            {
                return configList.ContainsConfig(key);
            }

            return false;
        }

        public IReadOnlyList<T> GetConfigs<T>() where T : IConfig
        {
            if (TryGetConfigList<T>(out IConfigList<T> configList))
            {
                return configList.GetConfigs();
            }

            return null;
        }

        public T GetConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfig<T>(key, out T configList))
            {
                return configList;
            }

            return default;
        }

        public bool TryGetConfig<T>(int key, out T value) where T : IConfig
        {
            if (TryGetConfigList<T>(out IConfigList<T> configList))
            {
                return configList.TryGetConfig(key, out value);
            }

            value = default;
            return false;
        }

        public bool ContainsSingletonConfig<T>() where T : ISingletonConfig
        {
            return _singletonConfigTable.ContainsKey(typeof(T));
        }

        public T GetSingletonConfig<T>() where T : ISingletonConfig
        {
            if (TryGetSingletonConfig<T>(out T t))
            {
                return t;
            }

            return default;
        }

        public bool TryGetSingletonConfig<T>(out T value) where T : ISingletonConfig
        {
            if (_singletonConfigTable.TryGetValue(typeof(T), out ISingletonConfig singletonConfig) &&
                singletonConfig is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }
    }
}
