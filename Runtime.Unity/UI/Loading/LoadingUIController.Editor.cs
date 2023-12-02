#if UNITY_EDITOR && UNITY_2022_1_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace GBG.GameToolkit.Unity.UI
{
    partial class LoadingUIController
    {
        [CustomEditor(typeof(LoadingUIController))]
        class Editor : UnityEditor.Editor
        {
            private string _debugLocker = string.Empty;


            public override void OnInspectorGUI()
            {
                var ui = (LoadingUIController)target;
                if (!ui._fadeInCurve.IsNormalized())
                {
                    EditorGUILayout.HelpBox("The fade in curve must start at (0,0) and end at (1,1).", MessageType.Error);
                }

                if (!ui._fadeOutCurve.IsNormalized())
                {
                    EditorGUILayout.HelpBox("The fade out curve must start at (0,0) and end at (1,1).", MessageType.Error);
                }

                base.OnInspectorGUI();

                if (!Application.isPlaying)
                {
                    return;
                }

                EditorGUILayout.Space();

                _debugLocker = EditorGUILayout.TextField("[Debug] Locker", _debugLocker);

                if (GUILayout.Button("[Debug] Show"))
                {
                    var debugLocker = string.IsNullOrWhiteSpace(_debugLocker) ? null : _debugLocker;
                    ui.Show(locker: debugLocker);
                }

                if (GUILayout.Button("[Debug] Close"))
                {
                    var debugLocker = string.IsNullOrWhiteSpace(_debugLocker) ? null : _debugLocker;
                    ui.Close(locker: debugLocker);
                }

                if (GUILayout.Button("[Debug] Print Lockers"))
                {
                    PrintLockers();
                }
            }

            private void PrintLockers()
            {
                var ui = (LoadingUIController)target;

                Debug.Log($"Loading UI lockers: {ui.LockCount}.", this);
                foreach (var locker in ui._lockers)
                {
                    Debug.Log($"\t- {locker}", this);
                }
            }
        }
    }
}
#endif