using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.ConfigData
{
    public interface IConfigTables
    {
        bool ContainsConfigTable<T>() where T : IConfig;
        IConfigTable<T> GetConfigTable<T>() where T : IConfig;
        bool TryGetConfigTable<T>(out IConfigTable<T> configTable) where T : IConfig;

        bool ContainsConfig<T>(int key) where T : IConfig;
        IReadOnlyList<T> GetConfigs<T>() where T : IConfig;
        T GetConfig<T>(int key, T defaultValue = default) where T : IConfig;
        bool TryGetConfig<T>(int key, out T value) where T : IConfig;
    }

    public class DefaultConfigTables : IConfigTables
    {
        private IReadOnlyDictionary<Type, IConfigTablePtr> _configTables;


        public DefaultConfigTables(IReadOnlyDictionary<Type, IConfigTablePtr> configTables)
        {
            if (_configTables == null)
            {
                throw new ArgumentNullException(nameof(configTables), $"Argument '{nameof(configTables)} is null.");
            }

            foreach (var kv in configTables)
            {
                var keyType = kv.Key;
                var valueType = kv.Value.GetConfigType();
                if (keyType != valueType)
                {
                    throw new ArgumentException($"The config table element type '{valueType}' does not match type '{keyType}'.",
                        nameof(configTables));
                }
            }

            _configTables = configTables;
        }

        public bool ContainsConfigTable<T>() where T : IConfig
        {
            return _configTables.ContainsKey(typeof(T));
        }

        public IConfigTable<T> GetConfigTable<T>() where T : IConfig
        {
            if (TryGetConfigTable<T>(out var configTable))
            {
                return configTable;
            }

            return null;
        }

        public bool TryGetConfigTable<T>(out IConfigTable<T> configTable) where T : IConfig
        {
            if (_configTables.TryGetValue(typeof(T), out var tablePtr))
            {
                configTable = (IConfigTable<T>)tablePtr;
                return true;
            }

            configTable = null;
            return false;
        }

        public bool ContainsConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfigTable<T>(out var configTable))
            {
                return configTable.ContainsConfig(key);
            }

            return false;
        }

        public IReadOnlyList<T> GetConfigs<T>() where T : IConfig
        {
            if (TryGetConfigTable<T>(out var configTable))
            {
                return configTable.GetConfigs();
            }

            return null;
        }

        public T GetConfig<T>(int key, T defaultValue = default) where T : IConfig
        {
            if (TryGetConfig<T>(key, out var configTable))
            {
                return configTable;
            }

            return defaultValue;
        }

        public bool TryGetConfig<T>(int key, out T value) where T : IConfig
        {
            if (TryGetConfigTable<T>(out var configTable))
            {
                return configTable.TryGetConfig(key, out value);
            }

            value = default;
            return false;
        }
    }
}
