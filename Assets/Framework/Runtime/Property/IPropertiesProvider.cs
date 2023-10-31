namespace GBG.Framework.Property
{
    public interface IPropertiesProvider
    {
        bool ContainsProperty(int specId);
        double GetPropertyValue(int specId);
        bool TryGetPropertyValue(int specId, out double value);

        //bool ContainsPropertyModifier(int specId);
    }
}
