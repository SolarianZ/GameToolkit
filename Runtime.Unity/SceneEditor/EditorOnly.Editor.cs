#if UNITY_EDITOR
using GBG.GameToolkit.Unity.ConfigData;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Unity
{
    partial class EditorOnly
    {
        #region Static

        private static Texture2D _editorIcon;
        private static Texture2D _errorIcon;

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            GameObject instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (!instance)
            {
                return;
            }

            if (!instance.TryGetComponent(out EditorOnly _))
            {
                return;
            }

            Texture2D icon;
            if (!instance.CompareTag(Tag))
            {
                if (!_errorIcon)
                {
                    _errorIcon = (Texture2D)EditorGUIUtility.Load("Error");
                }

                icon = _errorIcon;
            }
            else
            {
                if (!_editorIcon)
                {
                    _editorIcon = Resources.Load<Texture2D>("Icons/Icon_UnityEditor_Small");
                }

                icon = _editorIcon;
            }

            Rect iconRect = new(selectionRect.xMax - selectionRect.height,
                selectionRect.yMin, selectionRect.height, selectionRect.height);
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleAndCrop);
        }

        #endregion


        [Tooltip("Used to automatically find config table asset.")]
        public string EditorConfigMode;

        internal static ConfigTableAsset EditorConfigTableAssetCache;


        private void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }


        public void EditorExportConfigs(ConfigTableAsset configTableAsset)
        {
            var configComps = GetComponentsInChildren<EditorConfig>();
            foreach (EditorConfig configComp in configComps)
            {
                configComp.ExportConfig(configTableAsset);
            }
        }
    }

    [CustomEditor(typeof(EditorOnly))]
    class EditorOnlyInspector : UnityEditor.Editor
    {
        private EditorOnly Target => (EditorOnly)target;


        private void OnEnable()
        {
            TrySearchConfigTableAsset();
        }

        private void TrySearchConfigTableAsset()
        {
            if (EditorOnly.EditorConfigTableAssetCache)
            {
                return;
            }

            var configTableAssets = Resources.FindObjectsOfTypeAll(typeof(ConfigTableAsset));
            if (configTableAssets.Length == 0)
            {
                return;
            }

            ConfigTableAsset configTableAsset;
            if (configTableAssets.Length == 1)
            {
                configTableAsset = (ConfigTableAsset)configTableAssets[0];
            }
            else
            {
                configTableAsset = (ConfigTableAsset)configTableAssets.FirstOrDefault(asset
                    => asset.name.Contains(Target.EditorConfigMode));
                if (!configTableAsset)
                {
                    configTableAsset = (ConfigTableAsset)configTableAssets[0];
                }
            }

            EditorOnly.EditorConfigTableAssetCache = configTableAsset;
        }

        public override void OnInspectorGUI()
        {
            if (!Target.IsTagValid())
            {
                string message = $"Game objects with the {nameof(EditorOnly)} component should use the tag '{EditorOnly.Tag}'.";
                EditorGUILayout.HelpBox(message, MessageType.Error);

                if (GUILayout.Button($"Set tag to '{EditorOnly.Tag}'"))
                {
                    Undo.RecordObject(Target.gameObject, $"Set tag to '{EditorOnly.Tag}'");
                    Target.gameObject.tag = EditorOnly.Tag;
                }
            }

            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                return;
            }

            EditorGUILayout.Space();

            EditorOnly.EditorConfigTableAssetCache = (ConfigTableAsset)EditorGUILayout.ObjectField("Config Table Asset",
                EditorOnly.EditorConfigTableAssetCache, typeof(ConfigTableAsset), false);

            if (GUILayout.Button("Export Configs"))
            {
                ExportConfigs();
            }
        }

        public void ExportConfigs()
        {
            if (!EditorOnly.EditorConfigTableAssetCache)
            {
                EditorUtility.DisplayDialog("Error",
                   "Please assign a config table asset before export.", "Ok");
                return;
            }

            Target.EditorExportConfigs(EditorOnly.EditorConfigTableAssetCache);
            UDebug.Log($"Export configs to '{EditorOnly.EditorConfigTableAssetCache}'.", Target);
        }
    }
}
#endif