#if UNITY_EDITOR
using UnityEditor;

namespace GBG.GameToolkit.Unity
{
    [CustomEditor(typeof(NamedSceneObject))]
    internal class NamedSceneObjectEditor : UnityEditor.Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            if (string.IsNullOrEmpty(((NamedSceneObject)target).KeyName))
            {
                EditorGUILayout.HelpBox("Key name cannot be empty.", MessageType.Error);
            }

            base.OnInspectorGUI();
        }
    }
}
#endif