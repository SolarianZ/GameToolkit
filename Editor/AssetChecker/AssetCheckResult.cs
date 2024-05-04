using System;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    [Flags]
    public enum ResultType : uint
    {
        AllPass = 1U << 0,
        NotImportant = 1U << 1,
        Warning = 1U << 2,
        Error = 1U << 3,
        Exception = 1U << 4,
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