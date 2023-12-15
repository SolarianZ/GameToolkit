namespace GBG.GameToolkit.Property
{
    public delegate void PropertyChangedHandler(int? propertySpecId);

    public interface IPropertiesProvider
    {
        event PropertyChangedHandler PropertyChanged;

        bool ContainsProperty(int specId);
        double GetPropertyValue(int specId, double defaultValue, bool clamp = true);
        bool TryGetPropertyValue(int specId, out double value, bool clamp = true);

        //bool ContainsPropertyModifier(int specId);
    }
}
