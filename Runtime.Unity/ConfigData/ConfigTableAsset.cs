using GBG.GameToolkit.ConfigData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GBG.GameToolkit.Unity.ConfigData
{
    public abstract class ConfigTableAssetPtr : ScriptableObject, IConfigTablePtr, IValidatable
    {
        [TextArea(0, 3)]
        public string Comment;

        public abstract Type GetConfigType();

        public abstract void Validate([NotNull] List<ValidationResult> results);

        public abstract void DistinctConfigs();

        public abstract void DeleteMultiConfigs(IEnumerable<int> idList);

        public abstract void DeleteRangeConfigs(int startId, int endId);
    }

    public abstract class ConfigTableAsset<T> : ConfigTableAssetPtr, IConfigTable<T> where T : IConfig
    {
        public T[] Configs
        {
            get
            {
                return _configs;
            }
            set
            {
                _configs = value;
                _isDirty = true;
            }
        }

        [UnityEngine.Serialization.FormerlySerializedAs("Configs")]
        [SerializeField]
        private T[] _configs = Array.Empty<T>();
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


        public new void SetDirty()
        {
            _isDirty = true;
#pragma warning disable CS0618 // Type or member is obsolete
            base.SetDirty();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public override void Validate([NotNull] List<ValidationResult> results)
        {
            HashSet<int> idSet = new HashSet<int>();
            for (int i = 0; i < Configs.Length; i++)
            {
                T config = Configs[i];
                if (config == null)
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResult.ResultType.Error,
                        Content = $"Null config entry at index '{i}'.",
                        Context = this,
                    });
                }
                else if (!idSet.Add(config.Id))
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


        public override Type GetConfigType() => typeof(T);


        public override void DistinctConfigs()
        {
            Configs = Configs.Distinct(new ConfigDistinctComparer<T>()).ToArray();
        }

        public override void DeleteMultiConfigs(IEnumerable<int> idList)
        {
            Configs = Configs.Where(config => !idList.Contains(config.Id)).ToArray();
        }

        public override void DeleteRangeConfigs(int startId, int endId)
        {
            Assert.IsTrue(startId <= endId);
            Configs = Configs.Where(config => config.Id < startId || config.Id > endId).ToArray();
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

        public T GetConfig(int id)
        {
            PrepareTable();
            if (TryGetConfig(id, out T config))
            {
                return config;
            }

            return default;
        }

        public bool TryGetConfig(int id, out T config)
        {
            PrepareTable();
            return _table.TryGetValue(id, out config);
        }

        private void PrepareTable()
        {
            if (_table == null /*|| _table.Count != Configs.Length*/)
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
