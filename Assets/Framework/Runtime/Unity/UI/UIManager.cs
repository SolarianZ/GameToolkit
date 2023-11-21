#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using UnityEngine;

namespace GBG.Framework.Unity.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private RectTransform _generalNode;
        [SerializeField]
        private RectTransform _topNode;
        [SerializeField]
        private LoadingUIController _defaultLoadingUI;

        //private readonly Dictionary<string, IUIController> _uiTable = new();

        public void Show<T>(string uiName, T prefab) where T : MonoBehaviour, IUIController<T> { }

        public void Show(string uiName) { }

        public void Close(string uiName) { }

        public void ShowTop<T>(string uiName, T prefab) where T : MonoBehaviour, IUIController<T> { }

        public void ShowTop(string uiName) { }

        public void CloseTop(string uiName) { }

        public void ShowDefaultLoading(object locker = null, float? fadeInTime = null)
        {
            _defaultLoadingUI.Show(locker: locker, fadeInTime: fadeInTime);
        }

        public void CloseDefaultLoading(object locker = null, float? fadeOutTime = null)
        {
            _defaultLoadingUI.Close(locker: locker, fadeOutTime: fadeOutTime);
        }
    }
}
#endif