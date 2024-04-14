using GBG.GameToolkit.Unity.ConfigData;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBG.GameToolkit.Editor.ConfigData
{
    public static class EditorConfigAssetUtility
    {
        public static void DistinctConfigs(this ConfigTableAssetPtr configTable)
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            Undo.RecordObject(configTable, "Distinct Configs");
            configTable.DistinctConfigs();
            EditorUtility.SetDirty(configTable);
            // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
            AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(configTable));
            AssetDatabase.SaveAssetIfDirty(configTable);
        }

        public static void DeleteMultiConfigs(this ConfigTableAssetPtr configTable, IEnumerable<int> idList)
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            Undo.RecordObject(configTable, "Delete Multi Configs");
            configTable.DeleteMultiConfigs(idList);
            EditorUtility.SetDirty(configTable);
            // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
            AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(configTable));
            AssetDatabase.SaveAssetIfDirty(configTable);
        }

        public static void DeleteRangeConfigs(this ConfigTableAssetPtr configTable, int startId, int endId)
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            if (startId > endId)
            {
                EditorUtility.DisplayDialog("Error",
                    "The 'startId' cannot be greater than the 'endId'.", "Ok");
                return;
            }

            Undo.RecordObject(configTable, "Delete Range Configs");
            configTable.DeleteRangeConfigs(startId, endId);
            EditorUtility.SetDirty(configTable);
            // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
            AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(configTable));
            AssetDatabase.SaveAssetIfDirty(configTable);
        }
    }
}
