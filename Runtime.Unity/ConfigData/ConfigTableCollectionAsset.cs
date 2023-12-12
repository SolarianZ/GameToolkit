using System;
using System.Collections.Generic;
using GBG.GameToolkit.ConfigData;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [CreateAssetMenu(menuName = "Bamboo/Config Data/Config Table Collection")]
    public class ConfigTableCollectionAsset : ScriptableObject, IConfigTableProvider
    {
        public ConfigTableAssetPtr[] Configs = Array.Empty<ConfigTableAssetPtr>();

        private Dictionary<Type, ConfigTableAssetPtr> _table;


        private void PrepareTable()
        {
            if (_table != null)
            {
                return;
            }

            _table = new Dictionary<Type, ConfigTableAssetPtr>(Configs.Length);

            foreach (ConfigTableAssetPtr config in Configs)
            {
                _table.Add(config.GetConfigType(), config);
            }
        }

        public bool ContainsConfigTable<T>() where T : IConfig
        {
            PrepareTable();
            return _table.ContainsKey(typeof(T));
        }

        public IConfigTable<T> GetConfigTable<T>() where T : IConfig
        {
            if (TryGetConfigTable<T>(out IConfigTable<T> configTable))
            {
                return configTable;
            }

            return null;
        }

        public bool TryGetConfigTable<T>(out IConfigTable<T> configTable) where T : IConfig
        {
            PrepareTable();
            if (_table.TryGetValue(typeof(T), out ConfigTableAssetPtr tablePtr))
            {
                configTable = (IConfigTable<T>)tablePtr;
                return true;
            }

            configTable = null;
            return false;
        }


        public bool ContainsConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfigTable<T>(out IConfigTable<T> configTable))
            {
                return configTable.ContainsConfig(key);
            }

            return false;
        }

        public IReadOnlyList<T> GetConfigs<T>() where T : IConfig
        {
            if (TryGetConfigTable<T>(out IConfigTable<T> configTable))
            {
                return configTable.GetConfigs();
            }

            return null;
        }

        public T GetConfig<T>(int key, T defaultValue = default) where T : IConfig
        {
            if (TryGetConfig<T>(key, out T config))
            {
                return config;
            }

            return defaultValue;
        }

        public bool TryGetConfig<T>(int key, out T value) where T : IConfig
        {
            if (TryGetConfigTable<T>(out IConfigTable<T> configTable))
            {
                return configTable.TryGetConfig(key, out value);
            }

            value = default;
            return false;
        }
    }
}
