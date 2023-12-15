using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Property
{
    public class DefaultFlagsContainer : IFlagsContainer
    {
        public bool CacheFlags { get; set; }

        private HashSet<IFlagsProvider> _flagsProviders;
        private ulong? _flagsCache;

        public event Action FlagsChanged;


        public DefaultFlagsContainer(bool cacheFlags)
        {
            CacheFlags = cacheFlags;
        }

        public ulong GetFlags()
        {
            if (CacheFlags && _flagsCache != null)
            {
                return _flagsCache.Value;
            }

            ulong flags = 0;
            foreach (IFlagsProvider flagsProvider in _flagsProviders)
            {
                ulong innerFlags = flagsProvider.GetFlags();
                flags |= innerFlags;
            }

            if (CacheFlags)
            {
                _flagsCache = flags;
            }

            return flags;
        }

        public bool AddFlagsProvider(IFlagsProvider flagsProvider)
        {
            _flagsProviders ??= new HashSet<IFlagsProvider>();
            if (_flagsProviders.Add(flagsProvider))
            {
                flagsProvider.FlagsChanged += OnSubFlagsChanged;
                OnSubFlagsChanged();
            }

            return false;
        }

        public bool RemoveFlagsProvider(IFlagsProvider flagsProvider)
        {
            if (_flagsProviders == null)
            {
                return false;
            }

            if (_flagsProviders.Remove(flagsProvider))
            {
                flagsProvider.FlagsChanged -= OnSubFlagsChanged;
                OnSubFlagsChanged();
                return true;
            }

            return false;
        }

        private void OnSubFlagsChanged()
        {
            if (CacheFlags)
            {
                _flagsCache = null;
            }

            FlagsChanged?.Invoke();
        }
    }
}
