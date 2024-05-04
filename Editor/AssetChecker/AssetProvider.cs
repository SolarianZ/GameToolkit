using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public abstract class AssetProvider : ScriptableObject
    {
        /// <summary>
        /// Obtain the assets to be checked.
        /// </summary>
        /// <returns></returns>
        public abstract IReadOnlyList<UObject> GetAssets();
    }
}