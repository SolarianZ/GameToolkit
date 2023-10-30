namespace GBG.Framework.Property
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
}
