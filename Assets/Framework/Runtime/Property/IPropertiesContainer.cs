namespace GBG.GameToolkit.Property
{
    public interface IPropertiesContainer : IPropertiesProvider
    {
        bool AddProperty(object source, IProperty property);
        bool RemoveProperty(object source, IProperty property);

        //void AddPropertyModifier(IPropertyModifier modifier);
        //bool RemovePropertyModifier(IPropertyModifier modifier);
    }
}
