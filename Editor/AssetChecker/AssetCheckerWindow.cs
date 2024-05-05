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
                string errorMessage = $"No _asset provider specified in the settings.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
                return;
            }

            AssetChecker[] checkers = _settings.assetCheckers;
            if (checkers == null || checkers.Length == 0)
            {
                string errorMessage = $"No _asset checker specified in the settings.";
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
                            type = CheckResultType.Exception,
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
                string errorMessage = $"There are null _asset checkers in the settings, please check.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
            }
        }

        public void SetCheckResultTypeFilter(CheckResultType filter)
        {
            _resultFilterField.value = filter;
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
            string savePath = EditorUtility.SaveFilePanelInProject("Create new settings _asset",
                nameof(AssetCheckerSettings), "_asset", null);
            if (string.IsNullOrEmpty(savePath))
            {
                return null;
            }

            AssetCheckerSettings settings = CreateInstance<AssetCheckerSettings>();
            AssetDatabase.CreateAsset(settings, savePath);
            EditorGUIUtility.PingObject(settings);

            return settings;
        }

        private void OnAssetRechecked(AssetCheckResult newResult, AssetCheckResult oldResult)
        {
            bool clearSelection = false;
            if (newResult == null)
            {
                _checkResults.Remove(oldResult);
                clearSelection = true;
            }
            else
            {
                int resultIndex = _checkResults.IndexOf(oldResult);
                _checkResults[resultIndex] = newResult;
            }

            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(clearSelection);
        }

        private void OnAssetRepaired(AssetCheckResult newResult, bool allIssuesRepaired)
        {
            if (allIssuesRepaired)
            {
                _checkResults.Remove(newResult);
            }

            UpdatePersistentResultData();
            UpdateFilteredCheckResults();
            UpdateResultControls(allIssuesRepaired);
        }

        private void UpdatePersistentResultData()
        {
            _stats.Reset();
            for (int i = 0; i < _checkResults.Count; i++)
            {
                AssetCheckResult result = _checkResults[i];
                switch (result.type)
                {
                    case CheckResultType.AllPass:
                        _stats.allPass++;
                        break;
                    case CheckResultType.NotImportant:
                        _stats.notImportant++;
                        break;
                    case CheckResultType.Warning:
                        _stats.warning++;
                        break;
                    case CheckResultType.Error:
                        _stats.error++;
                        break;
                    case CheckResultType.Exception:
                        _stats.exception++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result.type), result.type, null);
                }
            }

            LocalCache.SetCheckResultStats(_stats);
            LocalCache.SetCheckResults(_checkResults);
        }

        private void UpdateFilteredCheckResults()
        {
            CheckResultType filter = LocalCache.GetCheckResultTypeFilter();
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