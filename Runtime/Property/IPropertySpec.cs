using GBG.GameToolkit.ConfigData;
using System;

namespace GBG.GameToolkit.Property
{
    public enum PropertyMergeMode
    {
        TakeTheSum,
        TakeTheMin,
        TakeTheMax,
    }

    public enum PropertyPosition
    {
        // (ΣBaseAddend)*(1+ΣMulAddend)+ΣRawAddend
        BaseAddend = 0,
        MulAddend = 100,
        RawAddend = 200,
    }

    public enum PropertyApproximation
    {
        Raw = 0,
        Floor = 100,
        Ceiling = 200,
        Round = 300,
    }

    public interface IPropertySpec
    {
        int Id { get; }
        double MinValue { get; }
        double MaxValue { get; }
        PropertyMergeMode MergeMode { get; }
        PropertyPosition Position { get; }
        PropertyApproximation ConvertMode { get; }

        double Clamp(double value)
        {
            return Math.Clamp(value, MinValue, MaxValue);
        }

        double Approximate(double value)
        {
            switch (ConvertMode)
            {
                case PropertyApproximation.Raw:
                    return value;
                case PropertyApproximation.Floor:
                    return Math.Floor(value);
                case PropertyApproximation.Ceiling:
                    return Math.Ceiling(value);
                case PropertyApproximation.Round:
                    return Math.Round(value);
                default:
                    throw new NotSupportedException($"Unknown PropertyApproximation '{ConvertMode}'.");
            }
        }
    }

    [Serializable]
    public class PropertySpec : IPropertySpec, IConfig
    {
        int IPropertySpec.Id => Id;
        int IConfig.Id => Id;
        double IPropertySpec.MinValue => MinValue;
        double IPropertySpec.MaxValue => MaxValue;
        PropertyMergeMode IPropertySpec.MergeMode => MergeMode;
        PropertyPosition IPropertySpec.Position => Position;
        PropertyApproximation IPropertySpec.ConvertMode => ConvertMode;

        public string Comment;
        public int Id;
        public double MinValue;
        public double MaxValue;
        public PropertyMergeMode MergeMode;
        public PropertyPosition Position;
        public PropertyApproximation ConvertMode;
    }
}
