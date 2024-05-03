using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public class AssetCheckerSettings : ScriptableObject
    {
        public AssetProvider assetProvider;
        public AssetChecker[] assetCheckers = Array.Empty<AssetChecker>();
    }
}