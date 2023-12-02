using System.Linq;
using GBG.GameToolkit.Unity.ConfigData;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableAsset))]
    public class ConfigTableAssetEditor : UnityEditor.Editor
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
            var assetGuids = AssetDatabase.FindAssets($"t:{nameof(ConfigAssetPtr)}");
            var configs = new ConfigAssetPtr[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; i++)
            {
                var assetGuid = assetGuids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath<ConfigAssetPtr>(assetPath);
                configs[i] = asset;
            }

            var configTableAsset = (ConfigTableAsset)target;
            Undo.RecordObject(configTableAsset, "Recollect Config Assets");
            configTableAsset.Configs = configs;
            EditorUtility.SetDirty(configTableAsset);
        }

        private void DistinctConfigAssets()
        {
            var configTableAsset = (ConfigTableAsset)target;
            Undo.RecordObject(configTableAsset, "Distinct Config Assets");
            configTableAsset.Configs.Distinct();
            EditorUtility.SetDirty(configTableAsset);
        }
    }
}
