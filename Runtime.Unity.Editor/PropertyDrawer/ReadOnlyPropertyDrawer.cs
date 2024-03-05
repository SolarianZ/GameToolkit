using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    [CustomPropertyDrawer(typeof(ReadOnlyInPlayModeAttribute))]
    [CustomPropertyDrawer(typeof(ReadOnlyInEditModeAttribute))]
    internal class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField gui = new PropertyField(property);
            ReadOnlyAttribute roAttr = (ReadOnlyAttribute)attribute;
            bool readOnly = Application.isPlaying ? roAttr.ReadOnlyInPlayMode : roAttr.ReadOnlyInEditMode;
            gui.SetEnabled(!readOnly);

            return gui;
        }

        // fo OnGUI only
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    return EditorGUI.GetPropertyHeight(property);
        //}
    }
}