#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using UnityEngine;

namespace GBG.Framework.Unity
{
    [DisallowMultipleComponent]
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;


        public static T GetInstance(bool createIfNotExists = false, string newInstanceName = null)
        {
            if (!_instance && createIfNotExists)
            {
                newInstanceName ??= $"[{typeof(T).Name}]";
                var go = new GameObject(newInstanceName);
                go.AddComponent<T>();
            }

            return _instance;
        }


        protected bool IsValidInstance { get; private set; }


        #region Unity Messages

        protected virtual void Awake()
        {
            if (!_instance)
            {
                _instance = (T)this;
                IsValidInstance = true;

                if (!transform.parent)
                {
                    DontDestroyOnLoad(this);
                }
            }
            else
            {
                enabled = false;
                IsValidInstance = false;
                Debug.LogError($"There is already another {typeof(T).Name} instance, disable this one({this}).", this);
            }
        }

        #endregion
    }
}
#endif