using System;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    [Serializable]
    public class AssetCheckResult
    {
        public CheckResultType type;
        public string title;
        public string details;
        public UObject asset;
        public AssetChecker checker;
        public bool repairable;
    }
}