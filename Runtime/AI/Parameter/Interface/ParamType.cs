namespace GBG.GameToolkit.AI.Parameter
{
    public enum ParamType : uint
    {
        Invalid = 0b0000_0000,


        #region Bacis value type 0b0001_0000

        Float32 = 0b0001_0100,
        //Float64 = 0b0001_0101,
        Int32 = 0b0001_1000,
        //Int64 =   0b0001_1001,
        Bool = 0b0001_1100,

        #endregion


        #region Complex value type 0b0000_1000

        Vector32 = 0b0000_1000,
        //Vector64 = 0b0000_1001,

        #endregion


        #region Object type 0b1000_0000

        Object = 0b1000_0000,
        String = 0b1000_0001,

        #endregion
    }

    public static class ParamTypeHelper
    {
        public static bool IsObjectType(this ParamType type)
        {
            return ((uint)type & 0b1000_0000) != 0;
        }
    }
}