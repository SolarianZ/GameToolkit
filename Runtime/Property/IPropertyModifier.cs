namespace GBG.GameToolkit.Property
{
    public enum PropertyModifyMode
    {
        Replace,
        PostProcess,
    }

    public interface IPropertyModifier
    {
        int SpecId { get; }
        PropertyModifyMode ModifyMode { get; }

        double ModifyProperty(double value);
    }
}
