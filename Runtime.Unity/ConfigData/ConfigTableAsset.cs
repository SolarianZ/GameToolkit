using GBG.GameToolkit.ConfigData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    public abstract class ConfigTableAssetPtr : ScriptableObject, IConfigTablePtr, IValidatable
    {
        [TextArea]
        public string Comment;

        public abstract Type GetConfigType();

        public abstract void Validate([NotNull] List<ValidationResult> results);

        public abstract void DistinctConfigs();
    }

    public abstract class ConfigTableAsset<T> : ConfigTableAssetPtr, IConfigTable<T> where T : IConfig
    {
        public T[] Configs = Array.Empty<T>();

        private Dictionary<int, T> _table;
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


        public override Type GetConfigType() => typeof(T);

        public override void Validate([NotNull] List<ValidationResult> results)
        {
            HashSet<int> idSet = new HashSet<int>();
            foreach (T config in Configs)
            {
                if (!idSet.Add(config.Id))
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Duplicate id: {config.Id}.",
                        Context = this,
                    });
                }
                else if (config.Id == 0)
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = "Invalid id: 0.",
                        Context = this,
                    });
                }
            }
        }

        public override void DistinctConfigs()
        {
            Configs = Configs.Distinct(new ConfigDistinctComparer<T>()).ToArray();
        }


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
            if (_table == null)
            {
                _table = new Dictionary<int, T>(Configs.Length);
                _isDirty = true;
            }

            if (!_isDirty)
            {
                return;
            }

            _table.Clear();
            foreach (T config in Configs)
            {
                if (!_table.TryAdd(config.Id, config))
                {
                    Debug.LogError($"Duplicate config id: {config.Id}. Config type: {typeof(T)}.", this);
                }
            }

            _isDirty = false;
        }
    }
}
