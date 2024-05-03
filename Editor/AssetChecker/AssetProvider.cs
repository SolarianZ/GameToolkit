using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public abstract class AssetProvider : ScriptableObject
    {
        public abstract IReadOnlyList<UObject> GetAssets();
    }
}