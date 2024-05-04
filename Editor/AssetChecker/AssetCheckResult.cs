using System;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    [Flags]
    public enum ResultType
    {
        AllPass = 1 << 0,
        NotImportant = 1 << 1,
        Warning = 1 << 2,
        Error = 1 << 3,
        Exception = 1 << 4,
    }

    [Serializable]
    public class AssetCheckResult
    {
        public ResultType type;
        public string title;
        public string details;
        public UObject asset;
        public AssetChecker checker;
        public bool repairable;
    }
}