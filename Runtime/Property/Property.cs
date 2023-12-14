namespace GBG.GameToolkit.Property
{
    public struct Property
    {
        //int Id;
        public int SpecId;
        public double Value;

        public Property(int specId, double value)
        {
            SpecId = specId;
            Value = value;
        }

        public override string ToString()
        {
            return $"#{SpecId} {Value:F5}";
        }
    }
}
