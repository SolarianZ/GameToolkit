using GBG.GameToolkit.Unity.ConfigData;
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
            AssetDatabase.SaveAssetIfDirty(configTable);
        }
    }
}
