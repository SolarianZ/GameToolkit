using UnityEngine;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public abstract class AssetChecker : ScriptableObject
    {
        /// <summary>
        /// Execute the _asset check process.
        /// </summary>
        /// <param name="asset">The _asset to be checked.</param>
        /// <returns></returns>
        public abstract AssetCheckResult CheckAsset(UObject asset);

        /// <summary>
        /// Attempt to repair the issues with the assets. 
        /// </summary>
        /// <param name="checkResult">Repair result. Can be null.</param>
        /// <param name="allIssuesRepaired">Whether all issues have been repaired.</param>
        public abstract void RepairAsset(AssetCheckResult checkResult, out bool allIssuesRepaired);
    }
}