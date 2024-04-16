using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableAsset))]
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

            var collectionAsset = (ConfigTableAsset)target;
            Undo.RecordObject(collectionAsset, "Recollect Config Assets");
            collectionAsset.SingletonConfigs = CollectAssetsOfType<SingletonConfigAssetPtr>();
            collectionAsset.ConfigLists = CollectAssetsOfType<ConfigListAssetPtr>();
            EditorUtility.SetDirty(collectionAsset);
            // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
            AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(collectionAsset));
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

            var table = (ConfigTableAsset)target;
            Undo.RecordObject(table, "Distinct Config Assets");
            table.SingletonConfigs = table.SingletonConfigs.Distinct().ToArray();
            table.ConfigLists = table.ConfigLists.Distinct().ToArray();
            EditorUtility.SetDirty(table);
            // MEMO Unity Bug UUM-66169: https://issuetracker.unity3d.com/issues/assetdatabase-dot-saveassetifdirty-does-not-automatically-check-out-assets
            AssetDatabase.MakeEditable(AssetDatabase.GetAssetPath(table));
            AssetDatabase.SaveAssetIfDirty(table);
        }

        private static T[] CollectAssetsOfType<T>() where T : UnityEngine.Object
        {
            string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            T[] assets = new T[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; i++)
            {
                var assetGuid = assetGuids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                assets[i] = asset;
            }

            return assets;
        }
    }
}
