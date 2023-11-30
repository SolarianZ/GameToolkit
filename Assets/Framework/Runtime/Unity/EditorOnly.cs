#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using UnityEngine;

namespace GBG.Framework.Unity
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    internal partial class EditorOnly : MonoBehaviour
    {
        public bool DeactivateOnEnterPlayMode = true;

        public bool IsTagValid()
        {
            return CompareTag("EditorOnly");
        }
    }
}
#endif