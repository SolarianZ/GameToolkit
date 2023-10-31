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

    public enum PropertyConvertion
    {
        Raw = 0, Floor = 100, Ceiling = 200, Round = 300,
    }

    public interface IPropertySpec : IConfig
    {
        int Id { get; }
        double MinValue { get; }
        double MaxValue { get; }
        PropertyPosition Position { get; }
        PropertyConvertion Convertion { get; }

        double Clamp(double value)
        {
            return Math.Clamp(value, MinValue, MaxValue);
        }

        double Convert(double value)
        {
            switch (Convertion)
            {
                case PropertyConvertion.Raw:
                    return value;
                case PropertyConvertion.Floor:
                    return Math.Floor(value);
                case PropertyConvertion.Ceiling:
                    return Math.Ceiling(value);
                case PropertyConvertion.Round:
                    return Math.Round(value);
                default:
                    throw new NotSupportedException($"Unknown PropertyConvertion '{Convertion}'.");
            }
        }

        double GetFinalValue(double value)
        {
            return Clamp(Convert(value));
        }
    }

    public class PropertySpec : IPropertySpec
    {
        public int Id { get; internal set; }
        public double MinValue { get; internal set; }
        public double MaxValue { get; internal set; }
        public PropertyPosition Position { get; internal set; }
        public PropertyConvertion Convertion { get; internal set; }
    }
}
