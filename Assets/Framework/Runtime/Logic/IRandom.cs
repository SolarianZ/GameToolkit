namespace GBG.Framework.Logic
{
    public interface IRandom
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }

    public class Random : IRandom
    {
        private readonly System.Random _random;

        public Random(int seed) => _random = new System.Random(seed);
        public int Next() => _random.Next();
        public int Next(int maxValue) => _random.Next(maxValue);
        public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    }
}
