using UnityEditor;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif
#if !UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace GBG.GameToolkit.Unity.Editor
{
    [CustomPropertyDrawer(typeof(TagPopupAttribute))]
    internal class TagPopupPropertyDrawer : PropertyDrawer
    {
#if UNITY_2021_3_OR_NEWER
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement()
            {
                style = { flexDirection = FlexDirection.Row }
            };

            TagField tagField = new TagField()
            {
                style = { flexGrow = 1 }
            };
            tagField.BindProperty(property);
            tagField.TrackPropertyValue(property, OnPropertyValueChanged);
            container.Add(tagField);

            Button clearButton = new Button(OnClearButtonClicked) { text = "Clear" };
            container.Add(clearButton);

            return container;


            void OnClearButtonClicked()
            {
                // TODO FIXME: Property value cleared but the tagField will not refresh until re-open its inspector
                property.stringValue = string.Empty;
                property.serializedObject.ApplyModifiedProperties();
                tagField.MarkDirtyRepaint();

                // TODO FIXME: Not work at all, even can't clear the value of the property
                //tagField.value = null;
                //tagField.MarkDirtyRepaint();
            }

            void OnPropertyValueChanged(SerializedProperty p)
            {
                // TODO FIXME: Not work
                //tagField.SetValueWithoutNotify(p.stringValue);
                //tagField.MarkDirtyRepaint();
            }
        }
#endif


        #region IMGUI
#if !UNITY_2021_3_OR_NEWER

        // Reference: Cinemachine.Editor.CinemachineTagFieldPropertyDrawer

        private GUIContent _clearText;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            const float hSpace = 2;
            _clearText ??= new GUIContent("Clear", "Set the tag to empty.");
            Vector2 textDimensions = GUI.skin.button.CalcSize(_clearText);
            rect.width -= textDimensions.x + hSpace;

            // Tag popup
            bool showMixedValueBak = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            string tagValue = property.stringValue;
            using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                label.text = null;
                using (new EditorGUI.PropertyScope(rect, label, property))
                {
                    tagValue = EditorGUI.TagField(rect, tagValue);
                    if (check.changed)
                    {
                        property.stringValue = tagValue;
                    }
                }
            }
            EditorGUI.showMixedValue = showMixedValueBak;

            // Clear button(useful when not in list)
            bool guiEnabledBak = GUI.enabled;
            GUI.enabled = tagValue.Length > 0;
            rect.x += rect.width + hSpace;
            rect.width = textDimensions.x;
            rect.height -= 1;
            if (GUI.Button(rect, _clearText))
            {
                property.stringValue = string.Empty;
            }
            GUI.enabled = guiEnabledBak;
        }

#endif
        #endregion
    }
}