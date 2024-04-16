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

        internal static ConfigTableAsset EditorConfigTableCollectionAssetCache;

        internal static void EditorSaveAllDirtyConfigAssets()
        {
            if (!EditorConfigTableCollectionAssetCache)
            {
                return;
            }

            foreach (ConfigListAssetPtr asset in EditorConfigTableCollectionAssetCache.ConfigLists)
            {
                if (EditorUtility.IsDirty(asset))
                {
                    // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
                    AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(asset));
                    AssetDatabase.SaveAssetIfDirty(asset);
                }
            }
        }


        private void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }


        public void EditorExportConfigs(ConfigTableAsset configTables, bool saveAssets)
        {
            if (!configTables)
            {
                Debug.LogError("Config table collection asset is null.", this);
                return;
            }

            var configComps = GetComponentsInChildren<EditorConfig>();
            foreach (EditorConfig configComp in configComps)
            {
                configComp.ExportConfig(configTables, saveAssets);
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
            if (EditorOnly.EditorConfigTableCollectionAssetCache)
            {
                return;
            }

            var configCollections = Resources.FindObjectsOfTypeAll(typeof(ConfigTableAsset));
            if (configCollections.Length == 0)
            {
                return;
            }

            ConfigTableAsset configTables;
            if (configCollections.Length == 1)
            {
                configTables = (ConfigTableAsset)configCollections[0];
            }
            else
            {
                configTables = (ConfigTableAsset)configCollections.FirstOrDefault(asset
                    => asset.name.Contains(Target.EditorConfigMode));
                if (!configTables)
                {
                    configTables = (ConfigTableAsset)configCollections[0];
                }
            }

            EditorOnly.EditorConfigTableCollectionAssetCache = configTables;
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

            EditorOnly.EditorConfigTableCollectionAssetCache = (ConfigTableAsset)EditorGUILayout.ObjectField("Config Table Collection",
                EditorOnly.EditorConfigTableCollectionAssetCache, typeof(ConfigTableAsset), false);

            if (GUILayout.Button("Export ConfigLists"))
            {
                ExportConfigs();
            }
        }

        public void ExportConfigs()
        {
            if (!EditorOnly.EditorConfigTableCollectionAssetCache)
            {
                EditorUtility.DisplayDialog("Error",
                   "Please assign a config table asset before export.", "Ok");
                return;
            }

            Target.EditorExportConfigs(EditorOnly.EditorConfigTableCollectionAssetCache, false);
            EditorOnly.EditorSaveAllDirtyConfigAssets();
            UDebug.Log($"Export configs to '{EditorOnly.EditorConfigTableCollectionAssetCache}'.", Target);
        }
    }
}
#endif