using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableCollectionAsset))]
    public class ConfigTableCollectionAssetEditor : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            EditorValidationUtility.CreateValidationResultViewsAndDefaultInspector(serializedObject, this,
                out var rootContainer, out _,
                out var validationResultListView, out _);
            ValidationResultListView = validationResultListView;

            var recollectButton = new Button(RecollectConfigAssets)
            {
                name = "RecollectConfigAssetsButton",
                text = "Recollect Config Assets",
                style =
                {
                    marginTop = 10,
                }
            };
            recollectButton.AddToClassList("recollect-config-assets-button");
            rootContainer.Add(recollectButton);

            var distinctButton = new Button(DistinctConfigAssets)
            {
                name = "DistinctConfigAssetsButton",
                text = "Distinct Config Assets",
            };
            distinctButton.AddToClassList("distinct-config-assets-button");
            rootContainer.Add(distinctButton);

            return rootContainer;
        }

        private void RecollectConfigAssets()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            var assetGuids = AssetDatabase.FindAssets($"t:{nameof(ConfigTableAssetPtr)}");
            var configTables = new ConfigTableAssetPtr[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; i++)
            {
                var assetGuid = assetGuids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath<ConfigTableAssetPtr>(assetPath);
                configTables[i] = asset;
            }

            var collectionAsset = (ConfigTableCollectionAsset)target;
            Undo.RecordObject(collectionAsset, "Recollect Config Assets");
            collectionAsset.ConfigTables = configTables;
            EditorUtility.SetDirty(collectionAsset);
            AssetDatabase.SaveAssetIfDirty(collectionAsset);
        }

        private void DistinctConfigAssets()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            var collectionAsset = (ConfigTableCollectionAsset)target;
            Undo.RecordObject(collectionAsset, "Distinct Config Assets");
            collectionAsset.ConfigTables = collectionAsset.ConfigTables.Distinct().ToArray();
            EditorUtility.SetDirty(collectionAsset);
            AssetDatabase.SaveAssetIfDirty(collectionAsset);
        }
    }
}
