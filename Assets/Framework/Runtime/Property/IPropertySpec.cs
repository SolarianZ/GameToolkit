using GBG.Framework.ConfigData;
using System;

namespace GBG.Framework.Property
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

    public class PropertySpec : IPropertySpec, IConfig
    {
        public int Id { get; internal set; }
        public double MinValue { get; internal set; }
        public double MaxValue { get; internal set; }
        public PropertyPosition Position { get; internal set; }
        public PropertyConvertMode ConvertMode { get; internal set; }
    }
}
