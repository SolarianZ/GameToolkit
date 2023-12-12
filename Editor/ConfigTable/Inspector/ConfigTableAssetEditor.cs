using GBG.GameToolkit.Unity.ConfigData;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableCollectionAsset))]
    public class ConfigTableCollectionAssetEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var rootContainer = new VisualElement
            {
                name = "RootContainer",
            };

            var defaultEditor = new VisualElement
            {
                name = "Default Editor",
            };
            InspectorElement.FillDefaultInspector(defaultEditor, serializedObject, this);
            rootContainer.Add(defaultEditor);

            var recollectButton = new Button(RecollectConfigAssets)
            {
                text = "Recollect Config Assets",
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
            var assetGuids = AssetDatabase.FindAssets($"t:{nameof(ConfigTableAssetPtr)}");
            var configs = new ConfigTableAssetPtr[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; i++)
            {
                var assetGuid = assetGuids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath<ConfigTableAssetPtr>(assetPath);
                configs[i] = asset;
            }

            var collectionAsset = (ConfigTableCollectionAsset)target;
            Undo.RecordObject(collectionAsset, "Recollect Config Assets");
            collectionAsset.Configs = configs;
            EditorUtility.SetDirty(collectionAsset);
        }

        private void DistinctConfigAssets()
        {
            var collectionAsset = (ConfigTableCollectionAsset)target;
            Undo.RecordObject(collectionAsset, "Distinct Config Assets");
            collectionAsset.Configs = collectionAsset.Configs.Distinct().ToArray();
            EditorUtility.SetDirty(collectionAsset);
        }
    }
}
