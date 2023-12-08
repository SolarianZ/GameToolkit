using System.Collections.Generic;
using GBG.GameToolkit.ConfigData;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [DisallowMultipleComponent]
    public abstract partial class EditorConfig : MonoBehaviour
    {
        [TextArea]
        public string Comment;
        public int Id;


        #region Unity Messages

        protected virtual void Reset() { }

        #endregion


        public string CommentOrId()
        {
            return string.IsNullOrEmpty(Comment) ? Id.ToString() : Comment;
        }

        public abstract void ExportConfig(ConfigTableAsset configTables);

        public static void SetConfig<TConfig, TAsset>(ConfigTableAsset configTables, TConfig config)
            where TConfig : IConfig
            where TAsset : ConfigAsset<TConfig>
        {
            if (!configTables.TryGetConfigTable<TConfig>(out var configTable))
            {
                Debug.LogError($"Cannot find table of type '{typeof(TConfig)}' in config table '{configTables}'.", configTables);
                return;
            }

            TAsset asset = (TAsset)configTable;
            List<TConfig> configs = new(asset.Configs);
            var oldIndex = configs.FindIndex(c => c.Id == config.Id);
            if (oldIndex > -1)
            {
                configs[oldIndex] = config;
            }
            else
            {
                configs.Add(config);
            }
            asset.Configs = configs.ToArray();
        }
    }
}
