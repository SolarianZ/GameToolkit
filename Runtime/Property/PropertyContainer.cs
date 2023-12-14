using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Property
{
    public class PropertyContainer : IPropertiesContainer
    {
        public bool CachePropertyValues { get; }

        private readonly IPropertySpecsProvider _propertySpecsProvider;
        private HashSet<IPropertiesProvider> _propertiesProviders;
        private Dictionary<int, double?> _propertyValueCaches;

        public event PropertyChangedHandler PropertyChanged;


        public PropertyContainer(IPropertySpecsProvider propertySpecsProvider, bool cachePropertyValues)
        {
            _propertySpecsProvider = propertySpecsProvider;
            CachePropertyValues = cachePropertyValues;
        }

        public bool ContainsProperty(int specId)
        {
            if (_propertyValueCaches?.ContainsKey(specId) ?? false)
            {
                return true;
            }

            foreach (IPropertiesProvider propertiesProvider in _propertiesProviders)
            {
                if (propertiesProvider.ContainsProperty(specId))
                {
                    return true;
                }
            }

            return false;
        }

        public double GetPropertyValue(int specId, double defaultValue)
        {
            if (TryGetPropertyValue(specId, out double value))
            {
                return value;
            }

            return defaultValue;
        }

        public bool TryGetPropertyValue(int specId, out double value)
        {
            if (CachePropertyValues && _propertyValueCaches != null &&
                _propertyValueCaches.TryGetValue(specId, out double? valueCache))
            {
                if (valueCache.HasValue)
                {
                    value = valueCache.Value;
                    return true;
                }

                value = default;
                return false;
            }

            IPropertySpec propertySpec = _propertySpecsProvider.GetPropertySpec(specId);
            if (propertySpec == null)
            {
                throw new ArgumentException($"Property spec '{specId}' does not exist.", nameof(specId));
            }

            value = 0;
            bool hasProperty = false;
            switch (propertySpec.MergeMode)
            {
                case PropertyMergeMode.TakeTheSum:
                    {
                        foreach (IPropertiesProvider propertiesProvider in _propertiesProviders)
                        {
                            if (propertiesProvider.TryGetPropertyValue(specId, out double innerValue))
                            {
                                hasProperty = true;
                                value += innerValue;
                            }
                        }
                        break;
                    }
                case PropertyMergeMode.TakeTheMin:
                    {
                        foreach (IPropertiesProvider propertiesProvider in _propertiesProviders)
                        {
                            if (propertiesProvider.TryGetPropertyValue(specId, out double innerValue))
                            {
                                hasProperty = true;
                                if (value > innerValue)
                                {
                                    value = innerValue;
                                }
                            }
                        }
                        break;
                    }
                case PropertyMergeMode.TakeTheMax:
                    {
                        foreach (IPropertiesProvider propertiesProvider in _propertiesProviders)
                        {
                            if (propertiesProvider.TryGetPropertyValue(specId, out double innerValue))
                            {
                                hasProperty = true;
                                if (value < innerValue)
                                {
                                    value = innerValue;
                                }
                            }
                        }
                        break;
                    }
                default:
                    throw new Exception($"Unknown property merge mode: {propertySpec.MergeMode}.");
            }

            if (CachePropertyValues)
            {
                _propertyValueCaches ??= new Dictionary<int, double?>();
                _propertyValueCaches[specId] = hasProperty ? value : null;
            }

            return hasProperty;
        }

        public bool AddPropertiesProvider(IPropertiesProvider propertiesProvider)
        {
            _propertiesProviders ??= new HashSet<IPropertiesProvider>();
            if (_propertiesProviders.Add(propertiesProvider))
            {
                propertiesProvider.PropertyChanged += OnSubPropertyChanged;
            }

            return false;
        }

        public bool RemovePropertiesProvider(IPropertiesProvider propertiesProvider)
        {
            if (_propertiesProviders == null)
            {
                return false;
            }

            if (_propertiesProviders.Remove(propertiesProvider))
            {
                propertiesProvider.PropertyChanged -= OnSubPropertyChanged;
                return true;
            }

            return false;
        }

        private void OnSubPropertyChanged(int specId)
        {
            if (CachePropertyValues && _propertyValueCaches != null)
            {
                _propertyValueCaches.Remove(specId);
            }

            PropertyChanged?.Invoke(specId);
        }
    }
}
