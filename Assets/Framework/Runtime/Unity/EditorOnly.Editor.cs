#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace GBG.Framework.Unity
{
    partial class EditorOnly
    {
        #region Static

        private static Texture2D _editorIcon;
        private static Texture2D _errorIcon;

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            GameObject instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (!instance)
            {
                return;
            }

            if (!instance.TryGetComponent(out EditorOnly _))
            {
                return;
            }

            Texture2D icon;
            if (!instance.CompareTag("EditorOnly"))
            {
                if (!_errorIcon)
                {
                    _errorIcon = (Texture2D)EditorGUIUtility.Load("Error");
                }

                icon = _errorIcon;
            }
            else
            {
                if (!_editorIcon)
                {
                    _editorIcon = Resources.Load<Texture2D>("Icons/Icon_UnityEditor_Small");
                }

                icon = _editorIcon;
            }

            Rect iconRect = new(selectionRect.xMax - selectionRect.height,
                selectionRect.yMin, selectionRect.height, selectionRect.height);
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleAndCrop);
        }

        #endregion


        private void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }

        private void Start()
        {
            if (DeactivateOnEnterPlayMode && Application.isPlaying)
            {
                gameObject.TrySetActive(false);
            }

            if (!IsTagValid())
            {
                UDebug.LogError($"The game object '{name}' has an {nameof(EditorOnly)} component, " +
                    $"but its tag is not 'EditorOnly'.", this);
            }
        }
    }

    [CustomEditor(typeof(EditorOnly))]
    class EditorOnlyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var comp = (EditorOnly)target;
            if (!comp.IsTagValid())
            {
                string message = $"Game objects with the {nameof(EditorOnly)} component should use the tag 'EditorOnly'.";
                EditorGUILayout.HelpBox(message, MessageType.Error);
            }

            base.OnInspectorGUI();
        }
    }
}
#endif