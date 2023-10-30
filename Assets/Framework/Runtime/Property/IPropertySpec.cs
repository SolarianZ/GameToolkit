using System;

namespace GBG.Framework.Property
{
    public enum PropertyPosition
    {
        // (base+p0)*(1+p1)+p2
        BaseAddend = 0, // p0
        MulAddend = 100, // p1
        RawAddend = 200, // p2
    }

    public enum PropertyConvertion
    {
        Raw = 0, Floor = 100, Ceiling = 200, Round = 300,
    }

    public interface IPropertySpec
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
}
