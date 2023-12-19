using GBG.GameToolkit.Property;
using System;

namespace GBG.GameToolkit.Ability.Buff
{
    partial class BuffInstanceBase : IPropertiesProvider, IFlagsProvider
    {
        #region Properties

        private readonly IPropertySpecsProvider _propertySpecsProvider;

        public event PropertyChangedHandler PropertyChanged;


        public virtual bool ProvideProperties()
        {
            return Config.Properties.Count > 0;
        }

        public virtual bool ProvideFlags()
        {
            return Config.Flags != 0;
        }

        public virtual bool ContainsProperty(int specId)
        {
            for (int i = 0; i < Config.Properties.Count; i++)
            {
                if (Config.Properties[i].SpecId == specId)
                {
                    return true;
                }
            }

            return false;
        }

        public double GetPropertyValue(int specId, double defaultValue, bool clamp = true)
        {
            if (TryGetPropertyValue(specId, out double value, clamp))
            {
                return value;
            }

            return defaultValue;
        }

        public virtual bool TryGetPropertyValue(int specId, out double value, bool clamp = true)
        {
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
                        for (int i = 0; i < Config.Properties.Count; i++)
                        {
                            Property.Property property = Config.Properties[i];
                            if (property.SpecId == specId)
                            {
                                hasProperty = true;
                                value += property.Value;
                            }
                        }
                        break;
                    }
                case PropertyMergeMode.TakeTheMin:
                    {
                        for (int i = 0; i < Config.Properties.Count; i++)
                        {
                            Property.Property property = Config.Properties[i];
                            if (property.SpecId == specId)
                            {
                                hasProperty = true;
                                if (value > property.Value)
                                {
                                    value = property.Value;
                                }
                            }
                        }
                        break;
                    }
                case PropertyMergeMode.TakeTheMax:
                    {
                        for (int i = 0; i < Config.Properties.Count; i++)
                        {
                            Property.Property property = Config.Properties[i];
                            if (property.SpecId == specId)
                            {
                                hasProperty = true;
                                if (value < property.Value)
                                {
                                    value = property.Value;
                                }
                            }
                        }
                        break;
                    }
                default:
                    throw new Exception($"Unknown property merge mode: {propertySpec.MergeMode}.");
            }

            if (clamp && hasProperty)
            {
                value = propertySpec.Clamp(value);
            }

            return hasProperty;
        }

        internal protected void RaisePropertyChangedEvent(int? propertySpecId)
        {
            PropertyChanged?.Invoke(propertySpecId);
        }

        #endregion


        #region Flags

        public event Action FlagsChanged;


        public virtual ulong GetFlags()
        {
            return Config.Flags;
        }

        internal protected void RaiseFlagsChangedEvent()
        {
            FlagsChanged?.Invoke();
        }

        #endregion
    }
}