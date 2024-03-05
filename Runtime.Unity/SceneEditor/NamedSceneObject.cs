using System.Collections.Generic;
using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Unity
{
    public class NamedSceneObject : MonoBehaviour
    {
        #region Static

        private static readonly Dictionary<string, NamedSceneObject> _globalObjectTable = new Dictionary<string, NamedSceneObject>();

        public static NamedSceneObject FindObject(string objectName)
        {
            if (_globalObjectTable.TryGetValue(objectName, out NamedSceneObject obj))
            {
                return obj;
            }

            return null;
        }

        public static T FindObject<T>(string objectName) where T : NamedSceneObject
        {
            if (_globalObjectTable.TryGetValue(objectName, out NamedSceneObject obj))
            {
                return obj as T;
            }

            return null;
        }

        #endregion


        public string KeyName
        {
            get => _uniqueKeyName;
            protected set => _uniqueKeyName = value;
        }

        [ReadOnlyInPlayMode]
        [SerializeField]
        private string _uniqueKeyName;
        private string _key;


        public void Refresh()
        {
            if (enabled)
            {
                Register();
            }
        }


        #region Unity Messages

        protected virtual void Reset()
        {
            _uniqueKeyName = name;
        }

        protected virtual void OnValidate()
        {
            // `OnValidate` will be called twice on enter play mode,
            // and on the first time the `Application.isPlaying` is false
            if (!Application.isPlaying && isActiveAndEnabled)
            {
                Register();
            }
        }

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        #endregion


        protected void Register()
        {
            if (_key == _uniqueKeyName)
            {
                return;
            }

            Unregister();

            _key = _uniqueKeyName;
            if (string.IsNullOrEmpty(_key))
            {
                UDebug.LogError("Register named scene object failed: name is empty.", this);
                return;
            }

            if (_globalObjectTable.ContainsKey(_key))
            {
                UDebug.LogError($"Register named scene object failed: duplicate name '{KeyName}'.", this);
                _key = null;
                return;
            }

            _globalObjectTable[_key] = this;
        }

        protected void Unregister()
        {
            if (string.IsNullOrEmpty(_key))
            {
                return;
            }

            if (_globalObjectTable.TryGetValue(_key, out NamedSceneObject obj))
            {
                if (obj != this)
                {
                    UDebug.LogError($"Unregister named scene object failed: unmatched name '{_key}'.", this);
                    return;
                }

                _globalObjectTable.Remove(_key);
                _key = null;
            }

            _globalObjectTable.Remove(_key);
            _key = null;
        }
    }
}