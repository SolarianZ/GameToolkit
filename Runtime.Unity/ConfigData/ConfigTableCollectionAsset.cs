using GBG.GameToolkit.ConfigData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [CreateAssetMenu(menuName = "Bamboo/Config Data/Config Table Collection")]
    public class ConfigTableCollectionAsset : ScriptableObject, IConfigTableProvider, IValidatable
    {
        [TextArea]
        public string Comment;
        [UnityEngine.Serialization.FormerlySerializedAs("ConfigTables")]
        public ConfigTableAssetPtr[] ConfigTables = Array.Empty<ConfigTableAssetPtr>();

        private Dictionary<Type, ConfigTableAssetPtr> _table;


        public virtual void Validate([NotNull] List<ValidationResult> results)
        {
            HashSet<ConfigTableAssetPtr> configSet = new HashSet<ConfigTableAssetPtr>();
            HashSet<Type> typeSet = new HashSet<Type>();
            foreach (ConfigTableAssetPtr configTable in ConfigTables)
            {
                if (!configTable)
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Config table reference is null.",
                        Context = this,
                    });
                    continue;
                }

                if (!configSet.Add(configTable))
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Duplicate config table asset: {configTable}.",
                        Context = this,
                    });
                }

                if (!typeSet.Add(configTable.GetConfigType()))
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Duplicate config table type: {configTable.GetConfigType()}.",
                        Context = this,
                    });
                }
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

        private void PrepareTable()
        {
            if (_table != null)
            {
                return;
            }

            _table = new Dictionary<Type, ConfigTableAssetPtr>(ConfigTables.Length);

            foreach (ConfigTableAssetPtr configTable in ConfigTables)
            {
                Type configType = configTable.GetConfigType();
                if (!_table.TryAdd(configType, configTable))
                {
                    Debug.LogError($"Duplicate config table type: {configType}.", this);
                }
            }
        }
    }
}
