using System;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    [Serializable]
    public class CheckResultStats : ICloneable
    {
        public int error;
        public int warning;
        public int notImportant;
        public int exception;

        internal CheckResultStats() { }

        public CheckResultStats(int error, int warning, int notImportant, int exception)
        {
            this.error = error;
            this.warning = warning;
            this.notImportant = notImportant;
            this.exception = exception;
        }

        public int GetTotal()
        {
            int total = error + warning + notImportant + exception;
            return total;
        }

        public void Reset()
        {
            error = 0;
            warning = 0;
            notImportant = 0;
            exception = 0;
        }

        public object Clone()
        {
            return new CheckResultStats(error, warning, notImportant, exception);
        }
    }
}