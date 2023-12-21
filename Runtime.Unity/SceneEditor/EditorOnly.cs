using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Unity
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public partial class EditorOnly : MonoBehaviour
    {
        public enum EnterPlayModeAction
        {
            None,
            Deactivate,
            Destroy,
        }


        public const string Tag = "EditorOnly";

        public EnterPlayModeAction PlayModeAction = EnterPlayModeAction.Destroy;


        private void Awake()
        {
            if (Application.isPlaying)
            {
                switch (PlayModeAction)
                {
                    case EnterPlayModeAction.None:
                        break;

                    case EnterPlayModeAction.Deactivate:
                        gameObject.TrySetActive(false);
                        break;

                    case EnterPlayModeAction.Destroy:
                        Destroy(gameObject);
                        break;

                    default:
                        throw new System.Exception($"Unknown enter play mode action: {PlayModeAction}.");
                }
            }

            if (!IsTagValid())
            {
                UDebug.LogError($"The game object '{name}' has an {nameof(EditorOnly)} component, " +
                    $"but its tag is not '{Tag}'.", this);
            }
        }

        public bool IsTagValid()
        {
            return CompareTag(Tag);
        }
    }
}
