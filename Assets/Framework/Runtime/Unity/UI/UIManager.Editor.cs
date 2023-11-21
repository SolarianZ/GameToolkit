#if UNITY_EDITOR && UNITY_2022_1_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace GBG.Framework.Unity.UI
{
    partial class UIManager
    {
        private bool _EDITOR_IsGroupMode = true;

        [CustomEditor(typeof(UIManager))]
        class Editor : UnityEditor.Editor
        {
            private string _debugGroupName = string.Empty;
            private sbyte _debugGroupPriority = 0;
            private string _debugUIName = string.Empty;
            private UIControllerBase _debugUIPrefab;
            private bool _debugIsTopUI;


            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Application.isPlaying)
                {
                    return;
                }

                EditorGUILayout.Space();
                var manager = (UIManager)target;

                // Mode
                EditorGUILayout.BeginHorizontal();
                var normalColor = GUI.color;
                var highlightColor = Color.yellow;
                GUI.color = manager._EDITOR_IsGroupMode ? highlightColor : normalColor;
                if (GUILayout.Button("Debug Group"))
                {
                    manager._EDITOR_IsGroupMode = true;
                }
                GUI.color = manager._EDITOR_IsGroupMode ? normalColor : highlightColor;
                if (GUILayout.Button("Debug UI"))
                {
                    manager._EDITOR_IsGroupMode = false;
                }
                GUI.color = normalColor;
                EditorGUILayout.EndHorizontal();

                if (manager._EDITOR_IsGroupMode)
                {
                    _debugGroupName = EditorGUILayout.TextField("[Debug] Group Name", _debugGroupName);
                    _debugGroupPriority = (sbyte)EditorGUILayout.IntSlider("[Debug] Group Priority", _debugGroupPriority, sbyte.MinValue, sbyte.MaxValue);

                    if (GUILayout.Button("[Debug] Create Group"))
                    {
                        manager.CreateGroup(_debugGroupName, _debugGroupPriority);
                    }

                    if (GUILayout.Button("[Debug] Delete Group"))
                    {
                        manager.DeleteGroup(_debugGroupName);
                    }
                }
                else
                {
                    _debugGroupName = EditorGUILayout.TextField("[Debug] Group Name", _debugGroupName);
                    _debugUIName = EditorGUILayout.TextField("[Debug] UI Name", _debugUIName);
                    _debugUIPrefab = (UIControllerBase)EditorGUILayout.ObjectField("[Debug] UI Prefab", _debugUIPrefab, typeof(UIControllerBase), true);
                    _debugIsTopUI = EditorGUILayout.Toggle("[Debug] Is Top UI", _debugIsTopUI);

                    if (GUILayout.Button("[Debug] Show UI"))
                    {
                        manager.Show(_debugUIName, _debugUIPrefab);
                    }

                    if (GUILayout.Button("[Debug] Close UI"))
                    {
                        manager.Close(_debugUIName);
                    }
                }
            }
        }
    }
}
#endif