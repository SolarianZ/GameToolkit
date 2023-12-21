using System.Collections.Generic;

namespace GBG.GameToolkit.ConfigData
{
    public class ConfigDistinctComparer<T> : IEqualityComparer<T> where T : IConfig
    {
        public bool Equals(T x, T y)
        {
            if (x == null)
            {
                return y == null;
            }

            if (y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id;
        }
    }
}
