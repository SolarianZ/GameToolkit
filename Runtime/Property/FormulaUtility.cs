using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Property
{
    public static class FormulaUtility
    {
        //private static IPropertySpecsProvider _propertySpecsProvider;
        //private static Dictionary<int, double> _tempPropertyValues;


        //public static void Initialize(IPropertySpecsProvider propertySpecsProvider)
        //{
        //    _propertySpecsProvider = propertySpecsProvider;
        //}

        //public static double AddUpProperties(IEnumerable<Property> properties, IPropertySpec customSpec = null)
        //{
        //    if (customSpec == null && _propertySpecsProvider == null)
        //    {
        //        throw new NullReferenceException("FormulaUtility is not initialized.");
        //    }

        //    int? specId = null;
        //    var spec = customSpec;
        //    var result = 0.0;
        //    foreach (var property in properties)
        //    {
        //        if (specId == null)
        //        {
        //            specId = property.SpecId;
        //            if (spec == null && !_propertySpecsProvider.TryGetValue(property.SpecId, out spec))
        //            {
        //                throw new ArgumentException($"PropertySpec '{property.SpecId}' does not exist.");
        //            }
        //        }
        //        else if (specId.Value != property.SpecId)
        //        {
        //            throw new ArgumentException("Property SpecIds are inconsistent.");
        //        }

        //        result += property.Value;
        //    }

        //    if (spec != null)
        //    {
        //        result = spec.Clamp(result);
        //    }

        //    return result;
        //}

        //public static double Evaluate(IEnumerable<Property> properties)
        //{
        //    if (_propertySpecsProvider == null)
        //    {
        //        throw new NullReferenceException("FormulaUtility is not initialized.");
        //    }


        //}

        //public static double CalculateProperties(IEnumerable<Property> properties)
        //{
        //    if (_propertySpecsProvider == null)
        //    {
        //        throw new NullReferenceException("FormulaUtility is not initialized.");
        //    }

        //    _tempPropertyValues ??= new Dictionary<int, double>(16);
        //    _tempPropertyValues.Clear();

        //    foreach (var property in properties)
        //    {
        //        if (_tempPropertyValues.TryGetValue(property.SpecId, out var value))
        //        {
        //            value += property.Value;
        //        }
        //        else
        //        {
        //            value = property.Value;
        //        }

        //        _tempPropertyValues[property.SpecId] = value;
        //    }

        //    var baseAddends = 0.0;
        //    var mulAddends = 0.0;
        //    var rawAddends = 0.0;
        //    foreach (var pair in _tempPropertyValues)
        //    {
        //        if (!_propertySpecsProvider.TryGetValue(pair.Key, out var spec))
        //        {
        //            throw new ArgumentException($"PropertySpec '{pair.Key}' does not exist.");
        //        }

        //        var value = spec.Clamp(pair.Value);
        //        switch (spec.Position)
        //        {
        //            case PropertyPosition.BaseAddend:
        //                baseAddends += value;
        //                break;
        //            case PropertyPosition.MulAddend:
        //                mulAddends += value;
        //                break;
        //            case PropertyPosition.RawAddend:
        //                rawAddends += value;
        //                break;
        //            default:
        //                throw new NotSupportedException($"Unknown PropertyPosition '{spec.Position}'.");
        //        }
        //    }

        //    var result = baseAddends * (1.0 + mulAddends) + rawAddends;
        //    return result;
        //}
    }
}
