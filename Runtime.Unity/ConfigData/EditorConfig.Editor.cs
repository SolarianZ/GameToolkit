#if UNITY_EDITOR
using GBG.GameToolkit.Unity.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [CustomEditor(typeof(EditorConfig), true)]
    class EditorConfigInspector : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            EditorValidationUtility.CreateValidationResultViewsAndDefaultInspector(serializedObject, this,
                out var rootContainer, out _,
                out var validationResultListView, out _);
            ValidationResultListView = validationResultListView;

            return rootContainer;
        }
    }
}
#endif