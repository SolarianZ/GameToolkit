#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UObject = UnityEngine.Object;

namespace GBG.Framework.Unity.ConfigData
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
        private static EditorConfigMessage[] EditorEmptyMessages { get; } = Array.Empty<EditorConfigMessage>();

        protected List<EditorConfigMessage> EditorValidationMessages { get; } = new();


        public virtual IReadOnlyList<EditorConfigMessage> Validate()
        {
            if (Id != 0)
            {
                return EditorEmptyMessages;
            }

            EditorValidationMessages.Clear();

            // Id
            EditorValidationMessages.Add(new EditorConfigMessage()
            {
                Type = MessageType.Error,
                Content = "'Id' cannot be '0'.",
                Context = this,
            });

            return EditorValidationMessages;
        }
    }

    [CustomEditor(typeof(EditorConfig), true)]
    class EditorConfigInspector : UnityEditor.Editor
    {
        private EditorConfig Target => (EditorConfig)target;


        public override void OnInspectorGUI()
        {
            // Validate
            IReadOnlyList<EditorConfigMessage> messages = Target.Validate();
            for (int i = 0; i < messages.Count; i++)
            {
                EditorConfigMessage message = messages[i];
                EditorGUILayout.HelpBox(message.Content, message.Type);
            }

            base.OnInspectorGUI();
        }
    }
}
#endif