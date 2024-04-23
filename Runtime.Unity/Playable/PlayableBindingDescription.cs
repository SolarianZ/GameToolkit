using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UObject = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GBG.GameToolkit.Unity
{
    public class PlayableBindingDescription : MonoBehaviour
    {
        public PlayableDirector PlayableDirector => _playableDirector;
        public IReadOnlyDictionary<BindingKeyInfo, UObject> BindingKeyTable => _bindingKeyTable;

        [SerializeField]
        private PlayableDirector _playableDirector;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Searchable]
        [ShowInInspector]
        [DictionaryDrawerSettings(KeyLabel = "Name & Type", ValueLabel = "Track")]
#endif
        private Dictionary<BindingKeyInfo, UObject> _bindingKeyTable = new Dictionary<BindingKeyInfo, UObject>();


        private void Reset()
        {
            SetPlayableDirector(GetComponent<PlayableDirector>());
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Start()
        {
            Refresh();
        }

        public void SetPlayableDirector(PlayableDirector director)
        {
            _playableDirector = director;
            Refresh();
        }

#if ODIN_INSPECTOR
        [Button("Refresh")]
#endif
        [ContextMenu("Refresh")]
        public void Refresh()
        {
            Debugger.LogInfo("Refresh generic bindings.", this, "GBG|PlayableBindingDescription");
            _playableDirector.CollectGenericBindings(_bindingKeyTable);
        }

        public void SetGenericBinding<T>(string streamName, T value) where T : UObject
        {
            if (!_playableDirector)
            {
                throw new NullReferenceException($"{nameof(PlayableDirector)} is not assigned.");
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, typeof(T));
            UObject sourceObject = _bindingKeyTable[key];
            _playableDirector.SetGenericBinding(sourceObject, value);
        }

        public bool TrySetGenericBinding<T>(string streamName, T value) where T : UObject
        {
            if (!_playableDirector)
            {
                return false;
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, typeof(T));
            if (_bindingKeyTable.TryGetValue(key, out UObject sourceObject))
            {
                _playableDirector.SetGenericBinding(sourceObject, value);
                return true;
            }

            return false;
        }

        public T GetGenericBinding<T>(string streamName) where T : UObject
        {
            if (!_playableDirector)
            {
                throw new NullReferenceException($"{nameof(PlayableDirector)} is not assigned.");
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, typeof(T));
            UObject sourceObject = _bindingKeyTable[key];
            UObject value = _playableDirector.GetGenericBinding(sourceObject);
            T t = (T)value;

            return t;
        }

        public bool TryGetGenericBinding<T>(string streamName, out T value) where T : UObject
        {
            if (!_playableDirector)
            {
                value = default;
                return false;
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, typeof(T));
            if (_bindingKeyTable.TryGetValue(key, out UObject sourceObject))
            {
                UObject bindingValue = _playableDirector.GetGenericBinding(sourceObject);
                if (bindingValue is T t)
                {
                    value = t;
                    return true;
                }
            }

            value = default;
            return false;
        }


    }
}