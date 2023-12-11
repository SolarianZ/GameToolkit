#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.ConfigData
{
    [Serializable]
    public struct EditorConfigMessage
    {
        public MessageType Type;
        public string Content;
        public UObject Context;
    }

    partial class EditorConfig
    {
        public virtual void EditorValidate(List<EditorConfigMessage> results)
        {
            if (Id != 0)
            {
                return;
            }

            // Id
            results.Add(new EditorConfigMessage()
            {
                Type = MessageType.Error,
                Content = "'Id' cannot be '0'.",
                Context = this,
            });
        }
    }

    [CustomEditor(typeof(EditorConfig), true)]
    class EditorConfigInspector : UnityEditor.Editor
    {
        private static List<EditorConfigMessage> _validationMessages = new();
        private EditorConfig Target => (EditorConfig)target;


        public override void OnInspectorGUI()
        {
            // EditorValidate
            _validationMessages.Clear();
            Target.EditorValidate(_validationMessages);
            for (int i = 0; i < _validationMessages.Count; i++)
            {
                EditorConfigMessage message = _validationMessages[i];
                EditorGUILayout.HelpBox(message.Content, message.Type);
            }

            base.OnInspectorGUI();
        }
    }
}
#endif