using GBG.GameToolkit.Property;
using System;

namespace GBG.GameToolkit.Ability.Buff
{
    partial class DefaultBuffManager<TBuffConfig> : IPropertiesContainer, IFlagsContainer
    {
        #region Properties

        private readonly DefaultPropertiesContainer _propertiesContainer;

        public event PropertyChangedHandler PropertyChanged
        {
            add => _propertiesContainer.PropertyChanged += value;
            remove => _propertiesContainer.PropertyChanged -= value;
        }


        public bool ContainsProperty(int specId)
        {
            return _propertiesContainer.ContainsProperty(specId);
        }

        public double GetPropertyValue(int specId, double defaultValue, bool clamp = true)
        {
            return _propertiesContainer.GetPropertyValue(specId, defaultValue, clamp);
        }

        public bool TryGetPropertyValue(int specId, out double value, bool clamp = true)
        {
            return _propertiesContainer.TryGetPropertyValue(specId, out value, clamp);
        }

        public bool AddPropertiesProvider(IPropertiesProvider propertiesProvider)
        {
            return _propertiesContainer.AddPropertiesProvider(propertiesProvider);
        }

        public bool RemovePropertiesProvider(IPropertiesProvider propertiesProvider)
        {
            return _propertiesContainer.RemovePropertiesProvider(propertiesProvider);
        }

        public void ClearAllPropertiesProviders()
        {
            _propertiesContainer.ClearAllPropertiesProviders();
        }


        #endregion


        #region Flags

        private readonly DefaultFlagsContainer _flagsContainer;

        public event Action FlagsChanged
        {
            add
            {
                _flagsContainer.FlagsChanged += value;
            }

            remove
            {
                _flagsContainer.FlagsChanged -= value;
            }
        }


        public ulong GetFlags()
        {
            return _flagsContainer.GetFlags();
        }

        public bool AddFlagsProvider(IFlagsProvider flagsProvider)
        {
            return _flagsContainer.AddFlagsProvider(flagsProvider);
        }

        public bool RemoveFlagsProvider(IFlagsProvider flagsProvider)
        {
            return _flagsContainer.RemoveFlagsProvider(flagsProvider);
        }

        public void ClearAllFlagsProviders()
        {
            _flagsContainer.ClearAllFlagsProviders();
        }

        #endregion
    }
}