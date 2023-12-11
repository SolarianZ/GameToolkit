using GBG.GameToolkit.ConfigData;
using System;
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
            for (int i = 0; i < asset.Configs.Length; i++)
            {
                if (asset.Configs[i].Id == config.Id)
                {
                    asset.Configs[i] = config;
                    return;
                }
            }

            TConfig[] configs = new TConfig[asset.Configs.Length + 1];
            Array.Copy(asset.Configs, configs, asset.Configs.Length);
            configs[asset.Configs.Length] = config;
            asset.Configs = configs;
        }
    }
}
