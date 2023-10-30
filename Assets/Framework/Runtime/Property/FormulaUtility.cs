using System;
using System.Collections.Generic;

namespace GBG.Framework.Property
{
    public static class FormulaUtility
    {
        private static IReadOnlyDictionary<int, IPropertySpec> _propertySpecs;
        private static Dictionary<int, double> _tempPropertyValues;


        public static void Initialize(IReadOnlyDictionary<int, IPropertySpec> propertySpecs)
        {
            _propertySpecs = propertySpecs;
        }

        public static double AddUpProperties(IEnumerable<IProperty> properties, IPropertySpec customSpec = null)
        {
            if (customSpec == null && _propertySpecs == null)
            {
                throw new NullReferenceException("FormulaUtility is not initialized.");
            }

            int? specId = null;
            var spec = customSpec;
            var result = 0.0;
            foreach (var property in properties)
            {
                if (specId == null)
                {
                    specId = property.SpecId;
                    if (spec == null && !_propertySpecs.TryGetValue(property.SpecId, out spec))
                    {
                        throw new ArgumentException($"PropertySpec '{property.SpecId}' does not exist.");
                    }
                }
                else if (specId.Value != property.SpecId)
                {
                    throw new ArgumentException("Property SpecIds are inconsistent.");
                }

                result += property.Value;
            }

            if (spec != null)
            {
                result = spec.Clamp(result);
            }

            return result;
        }

        public static double CalculateProperties(double baseValue, IEnumerable<IProperty> properties)
        {
            if (_propertySpecs == null)
            {
                throw new NullReferenceException("FormulaUtility is not initialized.");
            }

            _tempPropertyValues ??= new Dictionary<int, double>(16);
            _tempPropertyValues.Clear();

            foreach (var property in properties)
            {
                if (_tempPropertyValues.TryGetValue(property.SpecId, out var value))
                {
                    value += property.Value;
                }
                else
                {
                    value = property.Value;
                }

                _tempPropertyValues[property.SpecId] = value;
            }

            var baseAddends = 0.0;
            var mulAddends = 0.0;
            var rawAddends = 0.0;
            foreach (var pair in _tempPropertyValues)
            {
                if (!_propertySpecs.TryGetValue(pair.Key, out var spec))
                {
                    throw new ArgumentException($"PropertySpec '{pair.Key}' does not exist.");
                }

                var value = spec.GetFinalValue(pair.Value);
                switch (spec.Position)
                {
                    case PropertyPosition.BaseAddend:
                        baseAddends += value;
                        break;
                    case PropertyPosition.MulAddend:
                        mulAddends += value;
                        break;
                    case PropertyPosition.RawAddend:
                        rawAddends += value;
                        break;
                    default:
                        throw new NotSupportedException($"Unknown PropertyPosition '{spec.Position}'.");
                }
            }

            var result = (baseValue + baseAddends) * (1.0 + mulAddends) + rawAddends;
            return result;
        }
    }
}
