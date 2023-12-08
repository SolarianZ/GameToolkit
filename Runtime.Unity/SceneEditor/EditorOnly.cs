using UnityEngine;

namespace GBG.GameToolkit.Unity
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public partial class EditorOnly : MonoBehaviour
    {
        public bool DeactivateOnEnterPlayMode = true;

        public bool IsTagValid()
        {
            return CompareTag("EditorOnly");
        }
    }
}
