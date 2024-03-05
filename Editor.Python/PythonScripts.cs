using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity.Editor.Python
{
    [CreateAssetMenu(menuName = "Python Scripts Asset", fileName = "Python Scripts")]
    public class PythonScripts : ScriptableObject
    {
        public Content[] contents = Array.Empty<Content>();

        [Serializable]
        public class Content
        {
            [TextArea(0, 3)]
            public string comment;
            [TextArea(0, 20)]
            public string script;

            /// <inheritdoc />
            public override string ToString()
            {
                string c = comment.Trim().Replace("\n", "  ");
                return $"#{c}\n{script}";
            }
        }
    }
}