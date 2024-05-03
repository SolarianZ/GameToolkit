using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    [FilePath("Library/com.greenbamboogames.gametoolkit/AssetChecker",
        FilePathAttribute.Location.ProjectFolder)]
    internal class AssetCheckerLocalCache : ScriptableSingleton<AssetCheckerLocalCache>
    {
        [SerializeField]
        private AssetCheckerSettings _settingsAsset;
        [SerializeField]
        private AssetCheckResult[] _checkResults = Array.Empty<AssetCheckResult>();
        // TODO: Splitter position


        public AssetCheckerSettings GetSettingsAsset()
        {
            return _settingsAsset;
        }

        public void SetSettingsAsset(AssetCheckerSettings settings)
        {
            _settingsAsset = settings;

            Save(true);
        }

        public AssetCheckResult[] GetCheckResults()
        {
            return _checkResults.ToArray();
        }

        public void SetCheckResults(IEnumerable<AssetCheckResult> checkResults)
        {
            _checkResults = checkResults?.ToArray() ?? Array.Empty<AssetCheckResult>();

            Save(true);
        }
    }
}