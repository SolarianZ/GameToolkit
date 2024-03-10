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
        public ConfigTableAssetPtr[] ConfigTables
        {
            get { return _configTables; }
            set
            {
                _configTables = value;
                _isDirty = true;
            }
        }

        [TextArea(0, 3)]
        public string Comment;
        [UnityEngine.Serialization.FormerlySerializedAs("ConfigTables")]
        [SerializeField]
        private ConfigTableAssetPtr[] _configTables = Array.Empty<ConfigTableAssetPtr>();

        private Dictionary<Type, ConfigTableAssetPtr> _table;
        private bool _isDirty = true;


        #region Unity Messages

        protected virtual void OnValidate()
        {
            _isDirty = true;
        }

        protected virtual void Awake()
        {
            _isDirty = true;
        }

        #endregion


        public new void SetDirty()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            base.SetDirty();
#pragma warning restore CS0618 // Type or member is obsolete
            _isDirty = true;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

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

        public T GetConfig<T>(int key) where T : IConfig
        {
            if (TryGetConfig<T>(key, out T config))
            {
                return config;
            }

            return default;
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
            if (_table == null /*|| _table.Count != ConfigTables.Length*/)
            {
                _table = new Dictionary<Type, ConfigTableAssetPtr>(ConfigTables.Length);
                _isDirty = true;
            }

            if (!_isDirty)
            {
                return;
            }

            _table.Clear();

            foreach (ConfigTableAssetPtr configTable in ConfigTables)
            {
                Type configType = configTable.GetConfigType();
                if (!_table.TryAdd(configType, configTable))
                {
                    Debug.LogError($"Duplicate config table type: {configType}.", this);
                }

                _isDirty = false;
            }
        }
    }
}
