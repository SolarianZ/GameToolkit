using GBG.GameToolkit.Unity;
using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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
            var rootContainer = new VisualElement
            {
                name = "RootContainer",
            };

            var validationResultScroll = EditorValidationUtility.CreateValidationResultScrollView();
            ValidationResultListView = EditorValidationUtility.CreateSharedValidationResultListView();
            validationResultScroll.Add(ValidationResultListView);
            rootContainer.Add(validationResultScroll);

            var defaultEditor = new VisualElement
            {
                name = "Default Editor",
            };
            InspectorElement.FillDefaultInspector(defaultEditor, serializedObject, this);
            rootContainer.Add(defaultEditor);

            var recollectButton = new Button(RecollectConfigAssets)
            {
                text = "Recollect Config Assets",
                style =
                {
                    marginTop = 10,
                }
            };
            rootContainer.Add(recollectButton);

            var distinctButton = new Button(DistinctConfigAssets)
            {
                text = "Distinct Config Assets",
            };
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
