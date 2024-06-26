﻿using GBG.GameToolkit.ConfigData;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GBG.GameToolkit.Unity.ConfigData
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public abstract partial class EditorConfig : MonoBehaviour, IValidatable
    {
        [TextArea]
        public string Comment;
        public int Id;


        #region Unity Messages

        protected virtual void Reset() { }

        #endregion


        public virtual void Validate([NotNull] List<ValidationResult> results)
        {
            if (Id != 0)
            {
                return;
            }

            // Id
            results.Add(new ValidationResult()
            {
                Type = ValidationResult.ResultType.Error,
                Content = "'Id' cannot be '0'.",
                Context = this,
            });
        }

        public virtual string GenerateComment()
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

        public abstract void ExportConfig(ConfigTableAsset configTables, bool saveAsset = true);

        public static void SetConfig<TConfig>(ConfigTableAsset configTables, TConfig config,
            bool saveAsset = true, string undoName = null)
            where TConfig : IConfig
        {
            if (!configTables.TryGetConfigList<TConfig>(out var configTable))
            {
                Debug.LogError($"Cannot find table of type '{typeof(TConfig)}' in config table '{configTables}'.", configTables);
                return;
            }

            ConfigListAsset<TConfig> asset = (ConfigListAsset<TConfig>)configTable;
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(undoName))
            {
                Undo.RecordObject(asset, undoName);
            }
#endif

            for (int i = 0; i < asset.Configs.Length; i++)
            {
                // Override existing config item
                if (asset.Configs[i].Id == config.Id)
                {
                    asset.Configs[i] = config;
#if UNITY_EDITOR
                    EditorUtility.SetDirty(asset);
                    if (saveAsset)
                    {
                        // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
                        AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(asset));
                        AssetDatabase.SaveAssetIfDirty(asset);
                    }
#endif
                    return;
                }
            }

            // Add new config item
            TConfig[] configs = new TConfig[asset.Configs.Length + 1];
            Array.Copy(asset.Configs, configs, asset.Configs.Length);
            configs[asset.Configs.Length] = config;
            asset.Configs = configs;
#if UNITY_EDITOR
            EditorUtility.SetDirty(asset);
            if (saveAsset)
            {
                // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
                AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(asset));
                AssetDatabase.SaveAssetIfDirty(asset);
            }
#endif
        }
    }
}
