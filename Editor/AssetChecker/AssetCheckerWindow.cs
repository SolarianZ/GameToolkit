using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
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
        private readonly List<AssetCheckResult> _checkResults = new List<AssetCheckResult>();
        internal AssetCheckerLocalCache LocalCache => AssetCheckerLocalCache.instance;


        #region Unity Message

        private void OnEnable()
        {
            _settings = LocalCache.GetSettingsAsset();
            _checkResults.AddRange(LocalCache.GetCheckResults());
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

            IReadOnlyList<UObject> assets = _settings.assetProvider.GetAssets();
            if (assets == null || assets.Count == 0)
            {
                return;
            }

            _checkResults.Clear();
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

            if (hasNullChecker)
            {
                string errorMessage = $"There are null asset checkers in the settings, please check.";
                Debugger.LogError(errorMessage, _settings, LogTag);
                EditorUtility.DisplayDialog("Error", errorMessage, "Ok");
            }

            LocalCache.SetCheckResults(_checkResults);

            _resultListView.Rebuild();
            _resultListView.ClearSelection();
            _resultDetailsView.ClearSelection();
        }

        public AssetCheckResult[] GetCheckResults()
        {
            return _checkResults.ToArray();
        }

        public void ClearCheckResults()
        {
            _checkResults.Clear();
            LocalCache.SetCheckResults(_checkResults);

            _resultHelpBox.text = null;
            _resultHelpBox.style.display = DisplayStyle.None;
            _resultListView.Rebuild();
            _resultListView.ClearSelection();
            _resultDetailsView.ClearSelection();
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


        #region Custom Menu

        public void AddItemsToMenu(GenericMenu menu)
        {
            // Clear Check Results
            menu.AddItem(new GUIContent("Clear Check Results"), false, ClearCheckResults);

            // Debug
            menu.AddItem(new GUIContent("[Debug] Inspect Local Cache Asset"), false, () =>
            {
                Selection.activeObject = LocalCache;
            });
        }

        #endregion
    }
}