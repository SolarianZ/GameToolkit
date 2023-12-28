using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableAssetPtr), true)]
    public class ConfigTableAssetPtrEditor : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            EditorValidationUtility.CreateValidationResultViewsAndDefaultInspector(serializedObject, this,
                out var rootContainer, out _,
                out var validationResultListView, out _);
            ValidationResultListView = validationResultListView;

            var distinctButton = new Button(DistinctConfigs)
            {
                name = "DistinctConfigsButton",
                text = "Distinct Configs",
                style =
                {
                    marginTop = 10,
                }
            };
            distinctButton.AddToClassList("distinct-configs-button");
            rootContainer.Add(distinctButton);

            return rootContainer;
        }

        private void DistinctConfigs()
        {
            EditorConfigAssetUtility.DistinctConfigs((ConfigTableAssetPtr)target);
        }
    }
}
