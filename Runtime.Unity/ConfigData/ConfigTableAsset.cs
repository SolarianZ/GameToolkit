using GBG.GameToolkit.ConfigData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [CreateAssetMenu(menuName = "Bamboo/Config Data/Config Table Asset")]
    public class ConfigTableAsset : ScriptableObject, IConfigTable, IValidatable
    {
        public SingletonConfigAssetPtr[] SingletonConfigs
        {
            get { return _singletonConfigs; }
            set
            {
                _singletonConfigs = value;
                SetDirty();
            }
        }
        public ConfigListAssetPtr[] ConfigLists
        {
            get { return _configLists; }
            set
            {
                _configLists = value;
                SetDirty();
            }
        }

        [TextArea(0, 3)]
        public string Comment;
        [SerializeField]
        private SingletonConfigAssetPtr[] _singletonConfigs = Array.Empty<SingletonConfigAssetPtr>();
        [UnityEngine.Serialization.FormerlySerializedAs("ConfigLists")]
        [UnityEngine.Serialization.FormerlySerializedAs("_configTables")]
        [SerializeField]
        private ConfigListAssetPtr[] _configLists = Array.Empty<ConfigListAssetPtr>();

        private const string LogTag = "GBG|ConfigTableAsset";
        private Dictionary<Type, SingletonConfigAssetPtr> _singleConfigTable;
        private Dictionary<Type, ConfigListAssetPtr> _configTable;
        private bool _isDirty = true;


        #region Unity Messages

        protected virtual void OnValidate()
        {
            SetDirty();
        }

        protected virtual void Awake()
        {
            SetDirty();
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
            HashSet<ConfigListAssetPtr> configSet = new HashSet<ConfigListAssetPtr>();
            HashSet<Type> typeSet = new HashSet<Type>();
            for (var i = 0; i < ConfigLists.Length; i++)
            {
                ConfigListAssetPtr configList = ConfigLists[i];
                if (!configList)
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Null config list asset reference. Index: {i}.",
                        Context = this,
                    });
                    continue;
                }

                if (!configSet.Add(configList))
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Duplicate config list asset reference. Index: {i}.",
                        Context = this,
                    });
                }

                if (!typeSet.Add(configList.GetConfigType()))
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Duplicate config list type. Index: {i}, type: {configList.GetConfigType()}.",
                        Context = this,
                    });
                }
            }
        }


        public bool ContainsConfigList<T>() where T : IConfig
        {
            PrepareConfigTable();
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
            PrepareConfigTable();
            if (_configTable.TryGetValue(typeof(T), out ConfigListAssetPtr listPtr))
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
            if (TryGetConfig<T>(key, out T config))
            {
                return config;
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

        private void PrepareConfigTable()
        {
            if (_configTable == null /*|| _configTable.Count != ConfigLists.Length*/)
            {
                _configTable = new Dictionary<Type, ConfigListAssetPtr>(ConfigLists.Length);
                _isDirty = true;
            }

            if (!_isDirty)
            {
                return;
            }

            _configTable.Clear();

            for (var i = 0; i < ConfigLists.Length; i++)
            {
                ConfigListAssetPtr configList = ConfigLists[i];
                if (!configList)
                {
                    Debugger.LogError($"Null config list asset reference. Index: {i}.", this, LogTag);
                    continue;
                }

                Type configType = configList.GetConfigType();
                if (!_configTable.TryAdd(configType, configList))
                {
                    Debugger.LogError($"Duplicate config list type. Index: {i}, type: {configType}.", this, LogTag);
                }
            }

            _isDirty = false;
        }


        public bool ContainsSingletonConfig<T>() where T : ISingletonConfig
        {
            PrepareSingletonConfigTable();
            return _singleConfigTable.ContainsKey(typeof(T));
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
            PrepareSingletonConfigTable();

            if (!_singleConfigTable.TryGetValue(typeof(T), out SingletonConfigAssetPtr singletonConfig))
            {
                value = default;
                return false;
            }

            // Use ScriptableObject as a config object directly
            if (singletonConfig is T t)
            {
                value = t;
                return true;
            }

            // Wrap the config object in ScriptableObject
            if (singletonConfig is SingletonConfigAsset<T> tAsset)
            {
                value = tAsset.Config;
                return true;
            }

            value = default;
            return false;
        }

        private void PrepareSingletonConfigTable()
        {
            if (_singleConfigTable == null /*|| _singleConfigTable.Count != SingletonConfigs.Length*/)
            {
                _singleConfigTable = new Dictionary<Type, SingletonConfigAssetPtr>(SingletonConfigs.Length);
                _isDirty = true;
            }

            if (!_isDirty)
            {
                return;
            }

            _singleConfigTable.Clear();

            for (var i = 0; i < SingletonConfigs.Length; i++)
            {
                SingletonConfigAssetPtr config = SingletonConfigs[i];
                if (!config)
                {
                    Debugger.LogError($"Null singleton config asset reference. Index: {i}.", this, LogTag);
                    continue;
                }

                Type configType = config.GetConfigType();
                if (!_singleConfigTable.TryAdd(configType, config))
                {
                    Debugger.LogError($"Duplicate singleton config type. Index: {i}, type: {configType}.", this, LogTag);
                }
            }

            _isDirty = false;
        }
    }
}