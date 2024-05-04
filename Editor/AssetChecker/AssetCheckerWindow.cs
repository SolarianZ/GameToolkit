using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public partial class AssetCheckerWindow : EditorWindow, IHasCustomMenu
    {
        #region Static

        const string LogTag = "AssetChecker";


        [MenuItem("Tools/Bamboo/Asset Checker")]
        public static void Open()
        {
            GetWindow<AssetCheckerWindow>("AssetCheckerWindow");
        }

        #endregion


        [SerializeField]
        private AssetCheckerSettings _settings;
        private CheckResultStats _stats;
        private readonly List<AssetCheckResult> _checkResults = new List<AssetCheckResult>();
        private readonly List<AssetCheckResult> _filteredCheckResults = new List<AssetCheckResult>();
        internal AssetCheckerLocalCache LocalCache => AssetCheckerLocalCache.instance;


        #region Unity Message

        private void OnEnable()
        {
            _settings = LocalCache.GetSettingsAsset();
            _stats = LocalCache.GetCheckResultStats();
            _checkResults.AddRange(LocalCache.GetCheckResults());
            UpdateFilteredCheckResults();
        }

        private void OnFocus()
        {
            UpdateExecutionControls();
        }

        #endregion


        public void Execute()
        {
            Assert.IsTrue(_settings);

            if (!_settings.assetProvider)
            {
                string errorMessage = $"No asset provider specified in the settings.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
                return;
            }

            AssetChecker[] checkers = _settings.assetCheckers;
            if (checkers == null || checkers.Length == 0)
            {
                string errorMessage = $"No asset checker specified in the settings.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
                return;
            }

            _checkResults.Clear();
            IReadOnlyList<UObject> assets = _settings.assetProvider.GetAssets();
            if (assets == null || assets.Count == 0)
            {
                UpdatePersistentResultData();
                UpdateFilteredCheckResults();
                UpdateResultControls(true);
                return;
            }

            bool hasNullChecker = false;
            for (int i = 0; i < assets.Count; i++)
            {
                UObject asset = assets[i];
                for (int j = 0; j < checkers.Length; j++)
                {
                    AssetChecker checker = checkers[j];
                    if (!checker)
                    {
                        hasNullChecker = true;
                        continue;
                    }

                    try
                    {
                        AssetCheckResult result = checker.CheckAsset(asset);
                        if (result != null)
                        {
                            _checkResults.Add(result);
                        }
                    }
                    catch (Exception e)
                    {
                        AssetCheckResult result = new AssetCheckResult
                        {
                            type = ResultType.Exception,
                            title = e.GetType().Name,
                            details = e.Message,
                            asset = asset,
                            checker = checker,
                            repairable = false,
                        };
                        _checkResults.Add(result);
                    }
                }
            }

            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(true);

            if (hasNullChecker)
            {
                string errorMessage = $"There are null asset checkers in the settings, please check.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
            }
        }

        public void SetCheckResultTypeFilter(ResultType filter)
        {
            if (filter == LocalCache.GetCheckResultTypeFilter())
            {
                return;
            }

            LocalCache.SetCheckResultTypeFilter(filter);
            UpdateFilteredCheckResults();
            UpdateResultControls(true);
        }

        private void UpdatePersistentResultData()
        {
            _stats.Reset();
            for (int i = 0; i < _checkResults.Count; i++)
            {
                AssetCheckResult result = _checkResults[i];
                switch (result.type)
                {
                    case ResultType.AllPass:
                        _stats.allPass++;
                        break;
                    case ResultType.NotImportant:
                        _stats.notImportant++;
                        break;
                    case ResultType.Warning:
                        _stats.warning++;
                        break;
                    case ResultType.Error:
                        _stats.error++;
                        break;
                    case ResultType.Exception:
                        _stats.exception++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result.type), result.type, null);
                }
            }

            LocalCache.SetCheckResultStats(_stats);
            LocalCache.SetCheckResults(_checkResults);
        }

        public AssetCheckResult[] GetCheckResults()
        {
            return _checkResults.ToArray();
        }

        public void ClearCheckResults()
        {
            _checkResults.Clear();
            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(true);
        }

        public AssetCheckerSettings GetSettingsAsset()
        {
            return _settings;
        }

        public void SetSettingsAsset(AssetCheckerSettings settings)
        {
            _settingsField.value = settings;
        }

        public AssetCheckerSettings CreateSettingsAsset()
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Create new settings asset",
                nameof(AssetCheckerSettings), "asset", null);
            if (string.IsNullOrEmpty(savePath))
            {
                return null;
            }

            AssetCheckerSettings settings = CreateInstance<AssetCheckerSettings>();
            AssetDatabase.CreateAsset(settings, savePath);
            EditorGUIUtility.PingObject(settings);

            return settings;
        }

        private void OnAssetRechecked(int index)
        {
            bool clearSelection = false;
            if (_checkResults[index] == null)
            {
                _checkResults.RemoveAt(index);
                clearSelection = true;
            }

            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(clearSelection);
        }

        private void OnAssetRepaired(int index, bool success)
        {
            if (success)
            {
                _checkResults.RemoveAt(index);
            }

            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(success);
        }

        private void UpdateFilteredCheckResults()
        {
            ResultType filter = LocalCache.GetCheckResultTypeFilter();
            _filteredCheckResults.Clear();
            for (int i = 0; i < _checkResults.Count; i++)
            {
                AssetCheckResult result = _checkResults[i];
                if ((result.type & filter) != 0)
                {
                    _filteredCheckResults.Add(result);
                }
            }
        }


        #region Custom Menu

        public void AddItemsToMenu(GenericMenu menu)
        {
            // Clear Check Results
            menu.AddItem(new GUIContent("Clear Check Results"), false, ClearCheckResults);
            menu.AddSeparator("");

            // Result Icon Style
            menu.AddItem(new GUIContent("Result Icon Style/Style 1"),
                LocalCache.GetCheckResultIconStyle() == ResultIconStyle.Style1,
                () =>
                {
                    LocalCache.SetCheckResultIconStyle(ResultIconStyle.Style1);
                    _resultListView.Rebuild();
                });
            menu.AddItem(new GUIContent("Result Icon Style/Style 2"),
                LocalCache.GetCheckResultIconStyle() == ResultIconStyle.Style2,
                () =>
                {
                    LocalCache.SetCheckResultIconStyle(ResultIconStyle.Style2);
                    _resultListView.Rebuild();
                });
            menu.AddItem(new GUIContent("Result Icon Style/Style 3"),
                LocalCache.GetCheckResultIconStyle() == ResultIconStyle.Style3,
                () =>
                {
                    LocalCache.SetCheckResultIconStyle(ResultIconStyle.Style3);
                    _resultListView.Rebuild();
                });
            menu.AddSeparator("");

            // Debug
            menu.AddItem(new GUIContent("[Debug] Inspect Local Cache Asset"), false, () =>
            {
                Selection.activeObject = LocalCache;
            });
        }

        #endregion
    }
}