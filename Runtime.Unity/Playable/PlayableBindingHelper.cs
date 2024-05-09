using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity
{
    public static class PlayableBindingHelper
    {
        #region Collecting

        public static void CollectGenericBindings(this PlayableDirector playableDirector, Dictionary<BindingKey, UObject> bindingKeyTable)
        {
            if (bindingKeyTable == null)
            {
                throw new ArgumentNullException(nameof(bindingKeyTable));
            }

            PlayableAsset playableAsset = playableDirector ? playableDirector.playableAsset : null;
            playableAsset.CollectGenericBindings(bindingKeyTable);
        }

        public static void CollectGenericBindings(this PlayableAsset playableAsset, Dictionary<BindingKey, UObject> bindingKeyTable)
        {
            if (bindingKeyTable == null)
            {
                throw new ArgumentNullException(nameof(bindingKeyTable));
            }

            bindingKeyTable.Clear();
            if (!playableAsset)
            {
                return;
            }

            foreach (PlayableBinding binding in playableAsset.outputs)
            {
                if (HasGenericBinding(binding))
                {
                    BindingKey key = new BindingKey(binding.streamName, binding.outputTargetType);
                    if (bindingKeyTable.ContainsKey(key))
                    {
                        Debug.LogError($"Duplicate playable generic binding key: {key}.", playableAsset);
                        continue;
                    }

                    bindingKeyTable.Add(key, binding.sourceObject);
                }
            }
        }

        public static void CollectGenericBindings(this PlayableDirector playableDirector, List<BindingKey> bindingKeys)
        {
            if (bindingKeys == null)
            {
                throw new ArgumentNullException(nameof(bindingKeys));
            }

            PlayableAsset playableAsset = playableDirector ? playableDirector.playableAsset : null;
            playableAsset.CollectGenericBindings(bindingKeys);
        }

        public static void CollectGenericBindings(this PlayableAsset playableAsset, List<BindingKey> bindingKeys)
        {
            if (bindingKeys == null)
            {
                throw new ArgumentNullException(nameof(bindingKeys));
            }

            bindingKeys.Clear();
            if (!playableAsset)
            {
                return;
            }

            foreach (PlayableBinding binding in playableAsset.outputs)
            {
                if (HasGenericBinding(binding))
                {
                    bindingKeys.Add(new BindingKey(binding.streamName, binding.outputTargetType));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasGenericBinding(this PlayableBinding playableBinding)
        {
            return playableBinding.sourceObject && playableBinding.outputTargetType != null;
        }

        #endregion


        #region Binding

        /// <summary>
        /// Bind the object to playable track.
        /// </summary>
        /// <param name="playableDirector">PlayableDirector used for set generic binding.</param>
        /// <param name="bindingKeyTable">
        ///     Playable binding key table of the PlayableDirector.
        ///     Key: Playable binding key.
        ///     Value: Playable track.
        /// </param>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object used for bind to playable track.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void SetGenericBinding(PlayableDirector playableDirector,
            IReadOnlyDictionary<BindingKey, UObject> bindingKeyTable,
            BindingKey bindingKey, UObject bindingValue)
        {
            if (!playableDirector)
            {
                throw new ArgumentNullException(nameof(playableDirector));
            }

            if (bindingKeyTable == null)
            {
                throw new ArgumentNullException(nameof(bindingKeyTable));
            }

#if UNITY_EDITOR
            if (bindingValue)
            {
                Type declaredSourceBindingType = Type.GetType(bindingKey.OutputTargetTypeFullName);
                if (!declaredSourceBindingType.IsInstanceOfType(bindingValue))
                {
                    throw new ArgumentException($"Binding value must be instance of type '{bindingKey.OutputTargetTypeFullName}'.");
                }
            }
#endif

            UObject sourceObject = bindingKeyTable[bindingKey];
            playableDirector.SetGenericBinding(sourceObject, bindingValue);
        }

        /// <summary>
        /// Try bind the object to playable track.
        /// </summary>
        /// <param name="playableDirector">PlayableDirector used for set generic binding.</param>
        /// <param name="bindingKeyTable">
        ///     Playable binding key table of the PlayableDirector.
        ///     Key: Playable binding key.
        ///     Value: Playable track.
        /// </param>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object used for bind to playable track.</param>
        /// <returns></returns>
        public static bool TrySetGenericBinding(PlayableDirector playableDirector,
            IReadOnlyDictionary<BindingKey, UObject> bindingKeyTable,
            BindingKey bindingKey, UObject bindingValue)
        {
            if (!playableDirector || bindingKeyTable == null)
            {
                return false;
            }

            if (bindingKeyTable.TryGetValue(bindingKey, out UObject sourceObject))
            {
                playableDirector.SetGenericBinding(sourceObject, bindingValue);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the object bound to the playable track.
        /// </summary>
        /// <param name="playableDirector">PlayableDirector used for set generic binding.</param>
        /// <param name="bindingKeyTable">
        ///     Playable binding key table of the PlayableDirector.
        ///     Key: Playable binding key.
        ///     Value: Playable track.
        /// </param>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <returns>Object bound to the playable track.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public static UObject GetGenericBinding(PlayableDirector playableDirector,
            IReadOnlyDictionary<BindingKey, UObject> bindingKeyTable, BindingKey bindingKey)
        {
            if (!playableDirector)
            {
                throw new ArgumentNullException(nameof(playableDirector));
            }

            if (bindingKeyTable == null)
            {
                throw new ArgumentNullException(nameof(bindingKeyTable));
            }

            UObject sourceObject = bindingKeyTable[bindingKey];
            UObject bindingValue = playableDirector.GetGenericBinding(sourceObject);

            return bindingValue;
        }

        /// <summary>
        /// Try get the object bound to the playable track.
        /// </summary>
        /// <param name="playableDirector">PlayableDirector used for set generic binding.</param>
        /// <param name="bindingKeyTable">
        ///     Playable binding key table of the PlayableDirector.
        ///     Key: Playable binding key.
        ///     Value: Playable track.
        /// </param>
        /// <param name="bindingKey">Playable binding key.</param>
        /// <param name="bindingValue">Object bound to the playable track.</param>
        /// <returns></returns>
        public static bool TryGetGenericBinding(PlayableDirector playableDirector,
            IReadOnlyDictionary<BindingKey, UObject> bindingKeyTable,
            BindingKey bindingKey, out UObject bindingValue)
        {
            if (!playableDirector || bindingKeyTable == null)
            {
                bindingValue = default;
                return false;
            }

            if (bindingKeyTable.TryGetValue(bindingKey, out UObject sourceObject))
            {
                bindingValue = playableDirector.GetGenericBinding(sourceObject);
                return true;
            }

            bindingValue = default;
            return false;
        }

        #endregion
    }
}