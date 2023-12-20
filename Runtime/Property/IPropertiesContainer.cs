namespace GBG.GameToolkit.Property
{
    public interface IPropertiesContainer : IPropertiesProvider
    {
        bool AddPropertiesProvider(IPropertiesProvider propertiesProvider);
        bool RemovePropertiesProvider(IPropertiesProvider propertiesProvider);
        void ClearAllPropertiesProviders();

        //void AddPropertyModifier(IPropertyModifier modifier);
        //bool RemovePropertyModifier(IPropertyModifier modifier);
    }
}
