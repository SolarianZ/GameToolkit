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
        public IReadOnlyDictionary<BindingKey, UObject> BindingKeyTable => _bindingKeyTable;

#if ODIN_INSPECTOR
        [Required]
#endif
        [SerializeField]
        private PlayableDirector _playableDirector;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Searchable]
        [ShowInInspector]
        [DictionaryDrawerSettings(KeyLabel = "Name & Type", ValueLabel = "Track")]
#endif
        private Dictionary<BindingKey, UObject> _bindingKeyTable = new Dictionary<BindingKey, UObject>();


        private void Reset()
        {
            SetPlayableDirector(GetComponent<PlayableDirector>());
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void OnEnable()
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
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object used for bind to playable track.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetGenericBinding(BindingKey bindingKey, UObject bindingValue)
        {
            PlayableBindingHelper.SetGenericBinding(_playableDirector,
                _bindingKeyTable, bindingKey, bindingValue);
        }

        /// <summary>
        /// Try bind the object to playable track.
        /// </summary>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object used for bind to playable track.</param>
        /// <returns></returns>
        public bool TrySetGenericBinding(BindingKey bindingKey, UObject bindingValue)
        {
            return PlayableBindingHelper.TrySetGenericBinding(_playableDirector,
                _bindingKeyTable, bindingKey, bindingValue);
        }

        /// <summary>
        /// Get the object bound to the playable track.
        /// </summary>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <returns>Object bound to the playable track.</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public UObject GetGenericBinding(BindingKey bindingKey)
        {
            UObject bindingValue = PlayableBindingHelper.GetGenericBinding(_playableDirector,
                _bindingKeyTable, bindingKey);
            return bindingValue;
        }

        /// <summary>
        /// Try get the object bound to the playable track.
        /// </summary>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object bound to the playable track.</param>
        /// <returns></returns>
        public bool TryGetGenericBinding(BindingKey bindingKey, out UObject bindingValue)
        {
            return PlayableBindingHelper.TryGetGenericBinding(_playableDirector,
                _bindingKeyTable, bindingKey, out bindingValue);
        }
    }
}