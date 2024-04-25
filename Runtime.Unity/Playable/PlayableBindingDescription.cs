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

        /// <summary>
        /// Bind the object to playable track.
        /// </summary>
        /// <param name="streamName">Playable track name.</param>
        /// <param name="trackBindingType">Playable track binding type.</param>
        /// <param name="value">Object used for bind to playable track.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetGenericBinding(string streamName, Type trackBindingType, UObject value)
        {
            if (!_playableDirector)
            {
                throw new NullReferenceException($"{nameof(PlayableDirector)} is not assigned.");
            }

            if (streamName == null)
            {
                throw new ArgumentNullException(nameof(streamName));
            }

            if (trackBindingType == null)
            {
                throw new ArgumentNullException(nameof(trackBindingType));
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, trackBindingType);
            UObject sourceObject = _bindingKeyTable[key];
            _playableDirector.SetGenericBinding(sourceObject, value);
        }

        /// <summary>
        /// Try bind the object to playable track.
        /// </summary>
        /// <param name="streamName">Playable track name.</param>
        /// <param name="trackBindingType">Playable track binding type.</param>
        /// <param name="value">Object used for bind to playable track.</param>
        /// <returns></returns>
        public bool TrySetGenericBinding(string streamName, Type trackBindingType, UObject value)
        {
            if (!_playableDirector || streamName == null || trackBindingType == null)
            {
                return false;
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, trackBindingType);
            if (_bindingKeyTable.TryGetValue(key, out UObject sourceObject))
            {
                _playableDirector.SetGenericBinding(sourceObject, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the object bound to the playable track.
        /// </summary>
        /// <param name="streamName">Playable track name.</param>
        /// <param name="trackBindingType">Playable track binding type.</param>
        /// <returns>Object bound to the playable track.</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public UObject GetGenericBinding(string streamName, Type trackBindingType)
        {
            if (!_playableDirector)
            {
                throw new NullReferenceException($"{nameof(PlayableDirector)} is not assigned.");
            }

            if (streamName == null)
            {
                throw new ArgumentNullException(nameof(streamName));
            }

            if (trackBindingType == null)
            {
                throw new ArgumentNullException(nameof(trackBindingType));
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, trackBindingType);
            UObject sourceObject = _bindingKeyTable[key];
            UObject value = _playableDirector.GetGenericBinding(sourceObject);

            return value;
        }

        /// <summary>
        /// Try get the object bound to the playable track.
        /// </summary>
        /// <param name="streamName">Playable track name.</param>
        /// <param name="trackBindingType">Playable track binding type.</param>
        /// <param name="value">Object bound to the playable track.</param>
        /// <returns></returns>
        public bool TryGetGenericBinding(string streamName, Type trackBindingType, out UObject value)
        {
            if (!_playableDirector || streamName == null || trackBindingType == null)
            {
                value = default;
                return false;
            }

            BindingKeyInfo key = new BindingKeyInfo(streamName, trackBindingType);
            if (_bindingKeyTable.TryGetValue(key, out UObject sourceObject))
            {
                value = _playableDirector.GetGenericBinding(sourceObject);
                return true;
            }

            value = default;
            return false;
        }
    }
}