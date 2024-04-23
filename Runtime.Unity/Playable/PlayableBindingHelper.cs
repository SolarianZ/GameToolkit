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
        public static void CollectGenericBindings(this PlayableDirector playableDirector, Dictionary<BindingKeyInfo, UObject> bindingKeyTable)
        {
            if (bindingKeyTable == null)
            {
                throw new ArgumentNullException(nameof(bindingKeyTable));
            }

            PlayableAsset playableAsset = playableDirector ? playableDirector.playableAsset : null;
            playableAsset.CollectGenericBindings(bindingKeyTable);
        }

        public static void CollectGenericBindings(this PlayableAsset playableAsset, Dictionary<BindingKeyInfo, UObject> bindingKeyTable)
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
                    BindingKeyInfo key = new BindingKeyInfo(binding.streamName, binding.outputTargetType);
                    if (bindingKeyTable.ContainsKey(key))
                    {
                        Debug.LogError($"Duplicate playable generic binding key: {key}.", playableAsset);
                        continue;
                    }

                    bindingKeyTable.Add(key, binding.sourceObject);
                }
            }
        }

        public static void CollectGenericBindings(this PlayableDirector playableDirector, List<BindingKeyInfo> bindingKeys)
        {
            if (bindingKeys == null)
            {
                throw new ArgumentNullException(nameof(bindingKeys));
            }

            PlayableAsset playableAsset = playableDirector ? playableDirector.playableAsset : null;
            playableAsset.CollectGenericBindings(bindingKeys);
        }

        public static void CollectGenericBindings(this PlayableAsset playableAsset, List<BindingKeyInfo> bindingKeys)
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
                    bindingKeys.Add(new BindingKeyInfo(binding.streamName, binding.outputTargetType));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasGenericBinding(this PlayableBinding playableBinding)
        {
            return playableBinding.sourceObject && playableBinding.outputTargetType != null;
        }
    }
}