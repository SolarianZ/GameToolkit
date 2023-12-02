#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Collections.Generic;
using GBG.GameToolkit.ConfigData;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    public abstract class ConfigAssetPtr : ScriptableObject, IConfigTablePtr
    {
        public abstract Type GetConfigType();
    }

    public abstract class ConfigAsset<T> : ConfigAssetPtr, IConfigTable<T> where T : IConfig
    {
        public T[] Configs = Array.Empty<T>();

        private Dictionary<int, T> _table;


        public override Type GetConfigType() => typeof(T);

        public bool ContainsConfig(int key)
        {
            PrepareTable();
            return _table.ContainsKey(key);
        }

        public IReadOnlyList<T> GetConfigs()
        {
            return Configs;
        }

        public T GetConfig(int id, T defaultValue = default)
        {
            PrepareTable();
            if (TryGetConfig(id, out T config))
            {
                return config;
            }

            return defaultValue;
        }

        public bool TryGetConfig(int id, out T config)
        {
            PrepareTable();
            return _table.TryGetValue(id, out config);
        }

        private void PrepareTable()
        {
            if (_table != null)
            {
                return;
            }

            _table = new Dictionary<int, T>(Configs.Length);
            foreach (T config in Configs)
            {
                _table.Add(config.Id, config);
            }
        }
    }
}
#endif