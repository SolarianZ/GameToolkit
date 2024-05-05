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
        private CheckResultStats _checkResultStats = new CheckResultStats();
        [SerializeField]
        private AssetCheckResult[] _checkResults = Array.Empty<AssetCheckResult>();
        [SerializeField]
        private CheckResultType _resultTypeFilter = (CheckResultType)~0U;
        [SerializeField]
        private ResultIconStyle _resultIconStyle = ResultIconStyle.Style2;


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

        public CheckResultType GetCheckResultTypeFilter()
        {
            return _resultTypeFilter;
        }

        public void SetCheckResultTypeFilter(CheckResultType filter)
        {
            _resultTypeFilter = filter;
            Save(true);
        }

        public CheckResultStats GetCheckResultStats()
        {
            CheckResultStats stats = (CheckResultStats)_checkResultStats.Clone();
            return stats;
        }

        public void SetCheckResultStats(CheckResultStats stats)
        {
            _checkResultStats = stats == null
                ? new CheckResultStats()
                : (CheckResultStats)stats.Clone();
            Save(true);
        }

        public ResultIconStyle GetCheckResultIconStyle()
        {
            return _resultIconStyle;
        }

        public void SetCheckResultIconStyle(ResultIconStyle iconStyle)
        {
            _resultIconStyle = iconStyle;
            Save(true);
        }
    }
}