#if UNITY_EDITOR
using GBG.GameToolkit.Unity.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [CustomEditor(typeof(EditorConfig), true)]
    class EditorConfigInspector : ValidatableEditor
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

            return rootContainer;
        }
    }
}
#endif