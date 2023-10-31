using System;
using System.Collections.Generic;

namespace GBG.Framework.ConfigData
{
    public interface IConfigTables
    {
        void Reinitialize(IReadOnlyDictionary<Type, IReadOnlyDictionary<int, IConfig>> configTables);

        bool ContainsConfigTable<T>() where T : IConfig;
        IReadOnlyDictionary<int, IConfig> GetConfigTable<T>() where T : IConfig;
        bool TryGetConfigTable<T>(out IReadOnlyDictionary<int, IConfig> table) where T : IConfig;

        bool ContainsConfig<T>(int key) where T : IConfig;
        T GetConfig<T>(int key) where T : IConfig;
        bool TryGetConfig<T>(int key, out T value) where T : IConfig;
    }

    public class ConfigTables : IConfigTables
    {
        private IReadOnlyDictionary<Type, IReadOnlyDictionary<int, IConfig>> _configTables;


        public ConfigTables(IReadOnlyDictionary<Type, IReadOnlyDictionary<int, IConfig>> configTables)
        {
            Reinitialize(configTables);
        }

        public void Reinitialize(IReadOnlyDictionary<Type, IReadOnlyDictionary<int, IConfig>> configTables)
        {
            if (_configTables == null)
            {
                throw new ArgumentNullException(nameof(configTables), $"Argument '{nameof(configTables)} is null.");
            }

            _configTables = configTables;
        }

        public bool ContainsConfigTable<T>() where T : IConfig
        {
            return _configTables.ContainsKey(typeof(T));
        }

        public IReadOnlyDictionary<int, IConfig> GetConfigTable<T>() where T : IConfig
        {
            if (TryGetConfigTable<T>(out var table))
            {
                return table;
            }

            return null;
        }

        public bool TryGetConfigTable<T>(out IReadOnlyDictionary<int, IConfig> table) where T : IConfig
        {
            return _configTables.TryGetValue(typeof(T), out table);
        }

        public bool ContainsConfig<T>(int key) where T : IConfig
        {
            if (!_configTables.TryGetValue(typeof(T), out var table))
            {
                return false;
            }

            return table.ContainsKey(key);
        }

        public T GetConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfig<T>(key, out var config))
            {
                return config;
            }

            return default;
        }

        public bool TryGetConfig<T>(int key, out T value) where T : IConfig
        {
            if (!_configTables.TryGetValue(typeof(T), out var table))
            {
                value = default;
                return false;
            }

            if (!table.TryGetValue(key, out var config))
            {
                value = default;
                return false;
            }

            value = (T)config;
            return true;
        }
    }
}
