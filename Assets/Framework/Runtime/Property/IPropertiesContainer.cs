namespace GBG.Framework.Property
{
    public interface IPropertiesContainer : IPropertiesProvider
    {
        bool AddProperty(object source, IProperty property);
        bool RemoveProperty(object source, IProperty property);
    }
}
