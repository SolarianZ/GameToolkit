namespace GBG.GameToolkit.Property
{
    public static class FormulaUtility
    {
        /// <summary>
        /// BaseAddend*(1+MulAddend)+RawAddend.
        /// </summary>
        /// <param name="baseAddend"></param>
        /// <param name="mulAddend"></param>
        /// <param name="rawAddend"></param>
        /// <returns></returns>
        public static double Evaluate(double baseAddend, double mulAddend, double rawAddend)
        {
            double result = baseAddend * (1 + mulAddend) + rawAddend;
            return result;
        }
    }
}
