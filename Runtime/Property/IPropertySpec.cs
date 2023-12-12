using GBG.GameToolkit.ConfigData;
using System;

namespace GBG.GameToolkit.Property
{
    public enum PropertyPosition
    {
        // (ΣBaseAddend)*(1+ΣMulAddend)+ΣRawAddend
        BaseAddend = 0,
        MulAddend = 100,
        RawAddend = 200,
    }

    public enum PropertyConvertMode
    {
        Raw = 0, Floor = 100, Ceiling = 200, Round = 300,
    }

    public interface IPropertySpec
    {
        int Id { get; }
        double MinValue { get; }
        double MaxValue { get; }
        PropertyPosition Position { get; }
        PropertyConvertMode ConvertMode { get; }

        double Clamp(double value)
        {
            return Math.Clamp(value, MinValue, MaxValue);
        }

        double Convert(double value)
        {
            switch (ConvertMode)
            {
                case PropertyConvertMode.Raw:
                    return value;
                case PropertyConvertMode.Floor:
                    return Math.Floor(value);
                case PropertyConvertMode.Ceiling:
                    return Math.Ceiling(value);
                case PropertyConvertMode.Round:
                    return Math.Round(value);
                default:
                    throw new NotSupportedException($"Unknown PropertyConvertMode '{ConvertMode}'.");
            }
        }

        double GetFinalValue(double value)
        {
            return Clamp(Convert(value));
        }
    }

    [Serializable]
    public class PropertySpec : IPropertySpec, IConfig
    {
        int IPropertySpec.Id => Id;
        int IConfig.Id => Id;
        double IPropertySpec.MinValue => MinValue;
        double IPropertySpec.MaxValue => MaxValue;
        PropertyPosition IPropertySpec.Position => Position;
        PropertyConvertMode IPropertySpec.ConvertMode => ConvertMode;

        public int Id;
        public double MinValue;
        public double MaxValue;
        public PropertyPosition Position;
        public PropertyConvertMode ConvertMode;
    }
}
