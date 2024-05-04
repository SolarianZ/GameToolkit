﻿using GBG.GameToolkit.Unity.Editor.GUI;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public partial class AssetCheckerWindow
    {
        #region Controls

        private ObjectField _settingsField;
        private HelpBox _executionHelpBox;
        private Button _executeButton;
        private HelpBox _resultHelpBox;
        private ListView _resultListView;
        private CheckResultDetailsView _resultDetailsView;

        #endregion


        private void CreateGUI()
        {
            minSize = new Vector2(500, 400);
            VisualElement root = rootVisualElement;


            #region Settings

            // Settings Container
            VisualElement settingsContainer = new VisualElement
            {
                name = "SettingsContainer",
                style =
                {
                    paddingLeft = 4,
                    paddingRight = 4,
                    paddingTop = 4,
                    paddingBottom = 4,
                }
            };
            root.Add(settingsContainer);

            // Asset Container
            VisualElement settingsAssetContainer = new VisualElement
            {
                name = "SettingsAssetContainer",
                style =
                {
                    flexDirection = FlexDirection.Row,
                }
            };
            settingsContainer.Add(settingsAssetContainer);

            // Object Field
            _settingsField = new ObjectField
            {
                name = "SettingsField",
                label = "Settings",
                allowSceneObjects = false,
                objectType = typeof(AssetCheckerSettings),
                value = _settings,
                style =
                {
                    flexGrow = 1,
                }
            };
            _settingsField.bindingPath = nameof(_settings);
            _settingsField.RegisterValueChangedCallback(OnSettingsObjectChanged);
            settingsAssetContainer.Add(_settingsField);

            // Create Button
            Button createSettingsAssetButton = new Button
            {
                name = "CreateSettingsAssetButton",
                text = "New",
            };
            createSettingsAssetButton.clicked += () =>
            {
                _settings = CreateSettingsAsset();
            };
            settingsAssetContainer.Add(createSettingsAssetButton);

            #endregion


            #region Execution

            // Execution HelpBox
            _executionHelpBox = new HelpBox
            {
                name = "ExecutionHelpBox",
                messageType = HelpBoxMessageType.Error,
                style =
                {
                    marginLeft = 16,
                    marginRight = 16,
                }
            };
            _executionHelpBox.Q<Label>().style.fontSize = 13;
            root.Add(_executionHelpBox);

            // Execution Button
            _executeButton = new Button(Execute)
            {
                name = "ExecuteButton",
                text = "Execute",
                style =
                {
                    height = 28,
                    marginLeft = 8,
                    marginRight = 8,
                    marginTop = 8,
                    marginBottom = 8,
                }
            };
            root.Add(_executeButton);

            UpdateExecutionControls();

            #endregion

            // Separator
            root.Add(new VisualElement
            {
                name = "SettingsSeparator",
                style =
                {
                    backgroundColor = EditorGUIUtility.isProSkin
                        ? new Color(26 / 255f, 26 / 255f, 26 / 255f, 1.0f)
                        : new Color(127 / 255f, 127 / 255f, 127 / 255f, 1.0f),
                    width = StyleKeyword.Auto,
                    height = 1,
                    minHeight = 1,
                    maxHeight = 1,
                    marginLeft = 16,
                    marginRight = 16,
                }
            });

            // Execution HelpBox
            _resultHelpBox = new HelpBox
            {
                name = "ResultHelpBox",
                messageType = HelpBoxMessageType.Error,
                style =
                {
                    display = DisplayStyle.None,
                    marginLeft = 16,
                    marginRight = 16,
                }
            };
            _resultHelpBox.Q<Label>().style.fontSize = 13;
            root.Add(_resultHelpBox);

            // Result Container
            SplitterView resultContainer = new SplitterView(FlexDirection.Row)
            {
                name = "ResultContainer",
                Pane1 =
                {
                    name = "ResultItemContainer",
                    style =
                    {
                        width = 300,
                        minWidth = 200,
                    }
                },
                Pane2 =
                {
                    name = "ResultDetailsContainer",
                    style =
                    {
                        minWidth = 200,
                    }
                }
            };
            root.Add(resultContainer);


            #region Result List

            // Label
            Label resultLabel = new Label("Results")
            {
                name = "ResultLabel",
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            resultContainer.Pane1.Add(resultLabel);

            // Result List View
            _resultListView = new ListView
            {
                name = "ResultListView",
                itemsSource = _checkResults,
                fixedItemHeight = 28,
                selectionType = SelectionType.Single,
                makeItem = MakeResultListItem,
                bindItem = BindResultListItem,
            };
            _resultListView.selectedIndicesChanged += OnCheckResultSelectionChanged;
            resultContainer.Pane1.Add(_resultListView);

            #endregion


            #region Result Details

            // Label
            Label detailsLabel = new Label("Details")
            {
                name = "DetailsLabel",
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            resultContainer.Pane2.Add(detailsLabel);

            // Details View
            _resultDetailsView = new CheckResultDetailsView(_checkResults)
            {
                name = "CheckResultDetailsView",
            };
            _resultDetailsView.AssetRechecked += OnAssetRechecked;
            _resultDetailsView.AssetRepaired += OnAssetRepaired;
            resultContainer.Pane2.Add(_resultDetailsView);

            #endregion


            // SelectResult properties
            root.Bind(new SerializedObject(this));

            // Restore values
            // _settingsField.SetValueWithoutNotify(_settingsAsset);
        }

        private void OnSettingsObjectChanged(ChangeEvent<Object> evt)
        {
            LocalCache.SetSettingsAsset(_settings);
            UpdateExecutionControls();
        }

        private void UpdateExecutionControls()
        {
            if (_executionHelpBox == null || _executeButton == null)
            {
                return;
            }

            if (!_settings)
            {
                _executionHelpBox.messageType = HelpBoxMessageType.Error;
                _executionHelpBox.text = "Please specify settings asset.";
                _executionHelpBox.style.display = DisplayStyle.Flex;
                _executeButton.SetEnabled(false);
                return;
            }

            if (!_settings.assetProvider)
            {
                _executionHelpBox.messageType = HelpBoxMessageType.Error;
                _executionHelpBox.text = "No asset provider specified in the settings.";
                _executionHelpBox.style.display = DisplayStyle.Flex;
                _executeButton.SetEnabled(false);
                return;
            }

            AssetChecker[] checkers = _settings.assetCheckers;
            if (checkers == null || checkers.Length == 0)
            {
                _executionHelpBox.messageType = HelpBoxMessageType.Error;
                _executionHelpBox.text = "No asset checker specified in the settings.";
                _executionHelpBox.style.display = DisplayStyle.Flex;
                _executeButton.SetEnabled(false);
                return;
            }

            for (int i = 0; i < checkers.Length; i++)
            {
                AssetChecker checker = checkers[i];
                if (!checker)
                {
                    _executionHelpBox.messageType = HelpBoxMessageType.Warning;
                    _executionHelpBox.text = "There are null asset checkers in the settings, please check.";
                    _executionHelpBox.style.display = DisplayStyle.Flex;
                    return;
                }
            }

            _executionHelpBox.messageType = HelpBoxMessageType.Error;
            _executionHelpBox.text = "You should not see this message.";
            _executionHelpBox.style.display = DisplayStyle.None;
            _executeButton.SetEnabled(true);
        }

        private VisualElement MakeResultListItem()
        {
            CheckResultEntryView item = new CheckResultEntryView();
            return item;
        }

        private void BindResultListItem(VisualElement element, int index)
        {
            AssetCheckResult result = _checkResults[index];
            CheckResultEntryView item = (CheckResultEntryView)element;
            item.Bind(result);
        }

        private void OnCheckResultSelectionChanged(IEnumerable<int> selectedIndices)
        {
            int index = _resultListView.selectedIndex;
            _resultDetailsView.SelectResult(index);
        }

        private void OnAssetRechecked(int index)
        {
            LocalCache.SetCheckResults(_checkResults);
            _resultListView.RefreshItem(index);
        }

        private void OnAssetRepaired(int index)
        {
            LocalCache.SetCheckResults(_checkResults);
            _checkResults.RemoveAt(index);
            _resultListView.Rebuild();
            _resultListView.ClearSelection();
        }
    }
}