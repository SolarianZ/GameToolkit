using GBG.GameToolkit.Unity;
using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigTableAssetPtr), true)]
    public class ConfigTableAssetPtrEditor : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            var rootContainer = new VisualElement
            {
                name = "RootContainer",
            };
            rootContainer.AddToClassList("root-container__custom-inspector");

            var validationResultScroll = EditorValidationUtility.CreateValidationResultScrollView();
            ValidationResultListView = EditorValidationUtility.CreateSharedValidationResultListView();
            validationResultScroll.Add(ValidationResultListView);
            rootContainer.Add(validationResultScroll);

            var defaultEditor = new VisualElement
            {
                name = "DefaultEditor",
            };
            defaultEditor.AddToClassList("default-editor__inspector");
            InspectorElement.FillDefaultInspector(defaultEditor, serializedObject, this);
            rootContainer.Add(defaultEditor);

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
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error",
                    "This action cannot be performed in play mode.", "Ok");
                return;
            }

            ConfigTableAssetPtr configTable = (ConfigTableAssetPtr)target;
            Undo.RecordObject(configTable, "Distinct Configs");
            configTable.DistinctConfigs();
            EditorUtility.SetDirty(configTable);
            AssetDatabase.SaveAssetIfDirty(configTable);
        }
    }
}
