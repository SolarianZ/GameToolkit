using UnityEditor;
using UnityEngine;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    partial class AssetCheckerWindow
    {
        [SerializeField]
        private AssetCheckerSettings _settings;


        private void CreateSettingsAsset()
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Create new settings asset",
                nameof(AssetCheckerSettings), "asset", null);
            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }

            _settings = CreateInstance<AssetCheckerSettings>();
            AssetDatabase.CreateAsset(_settings, savePath);
            EditorGUIUtility.PingObject(_settings);
        }
    }
}