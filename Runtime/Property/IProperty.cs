namespace GBG.GameToolkit.Property
{
    public interface IProperty
    {
        //int Id { get; }
        int SpecId { get; }
        double Value { get; }

        string ToString()
        {
            return $"#{SpecId} {Value:F5}";
        }
    }

    public class Property : IProperty
    {
        public int SpecId { get; internal set; }
        public double Value { get; internal set; }

        public override string ToString()
        {
            return ((IProperty)this).ToString();
        }
    }
}
