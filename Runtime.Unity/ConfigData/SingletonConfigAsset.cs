using GBG.GameToolkit.ConfigData;
using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ConfigData
{
    public abstract class SingletonConfigAssetPtr : ScriptableObject, ISingletonConfig
    {
        [TextArea(1, 3)]
        public string Comment;


        public abstract Type GetConfigType();
    }

    public abstract class SingletonConfigAsset : SingletonConfigAssetPtr
    {
        public sealed override Type GetConfigType() => GetType();
    }

    public abstract class SingletonConfigAsset<T> : SingletonConfigAssetPtr where T : ISingletonConfig
    {
        public T Config;

        public sealed override Type GetConfigType() => typeof(T);
    }
}
