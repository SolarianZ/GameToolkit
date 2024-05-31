using System;

namespace GBG.GameToolkit.ConfigData
{
    public interface IConfig
    {
        public int Id { get; }
    }

    public interface IInt32EnumIdConfig<T> : IConfig where T : Enum
    {
        T EnumId { get; }
    }

    [Serializable]
    public abstract class Int32EnumIdConfig<T> : IInt32EnumIdConfig<T> where T : Enum
    {
        /// <inheritdoc />
        int IConfig.Id
        {
            get
            {
#if UNITY_EDITOR
                // Value may be modified from Inspector, do not cache
                SetEnumValueCacheDirty();
#endif
                _enumValueCache ??= ((IConvertible)EnumId).ToInt32(null);
                return _enumValueCache.Value;
            }
        }
        public abstract T EnumId { get; }
        private int? _enumValueCache;

        public void SetEnumValueCacheDirty() => _enumValueCache = null;
    }
}
