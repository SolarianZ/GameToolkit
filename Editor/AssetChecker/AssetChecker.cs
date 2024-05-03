using UnityEngine;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public abstract class AssetChecker : ScriptableObject
    {
        public abstract AssetCheckResult CheckAsset(UObject asset);

        public abstract bool TryRepairAsset(AssetCheckResult checkResult);
    }
}