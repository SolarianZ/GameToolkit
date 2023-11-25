#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using GBG.Framework.ConfigData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Framework.Unity.ConfigData
{
    [CreateAssetMenu(menuName = "Bamboo/Config Table Asset")]
    public class ConfigTableAsset : ScriptableObject, IConfigTables
    {
        public ConfigAssetPtr[] Configs = Array.Empty<ConfigAssetPtr>();

        private Dictionary<Type, ConfigAssetPtr> _table;


        private void PrepareTable()
        {
            if (_table != null)
            {
                return;
            }

            _table = new Dictionary<Type, ConfigAssetPtr>(Configs.Length);

            foreach (var config in Configs)
            {
                _table.Add(config.GetType(), config);
            }
        }

        public bool ContainsConfigTable<T>() where T : IConfig
        {
            PrepareTable();
            return _table.ContainsKey(typeof(T));
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
            PrepareTable();
            if (_table.TryGetValue(typeof(T), out var tablePtr))
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
            if (TryGetConfig<T>(key, out var config))
            {
                return config;
            }

            return defaultValue;
        }

        public bool TryGetConfig<T>(int key, out T value) where T : IConfig
        {
            if (_table.TryGetValue(typeof(T), out var tablePtr))
            {
                var configTable = (IConfigTable<T>)tablePtr;
                return configTable.TryGetConfig(key, out value);
            }

            value = default;
            return false;
        }
    }
}
#endif