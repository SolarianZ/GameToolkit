using GBG.GameToolkit.ConfigData;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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


        public string GetExportedComment()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                return $"#{Id} {name}";
            }

            if (Comment.StartsWith($"#{Id}", StringComparison.Ordinal))
            {
                return Comment;
            }

            return $"#{Id} {Comment}";
        }

        public abstract void ExportConfig(ConfigTableAsset configTables);

        public static void SetConfig<TConfig>(ConfigTableAsset configTables, TConfig config,
            bool setDirtyAndSave = true, string undoName = null)
            where TConfig : IConfig
        {
            if (!configTables.TryGetConfigTable<TConfig>(out var configTable))
            {
                Debug.LogError($"Cannot find table of type '{typeof(TConfig)}' in config table '{configTables}'.", configTables);
                return;
            }

            ConfigAsset<TConfig> asset = (ConfigAsset<TConfig>)configTable;
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(undoName))
            {
                Undo.RecordObject(asset, undoName);
            }
#endif

            for (int i = 0; i < asset.Configs.Length; i++)
            {
                if (asset.Configs[i].Id == config.Id)
                {
                    asset.Configs[i] = config;
#if UNITY_EDITOR
                    if (setDirtyAndSave)
                    {
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssetIfDirty(asset);
                    }
#endif
                    return;
                }
            }

            TConfig[] configs = new TConfig[asset.Configs.Length + 1];
            Array.Copy(asset.Configs, configs, asset.Configs.Length);
            configs[asset.Configs.Length] = config;
            asset.Configs = configs;
#if UNITY_EDITOR
            if (setDirtyAndSave)
            {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssetIfDirty(asset);
            }
#endif
        }
    }
}
