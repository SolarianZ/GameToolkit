using GBG.GameToolkit.Unity.Editor.GUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public partial class AssetCheckerWindow
    {
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
            _settingsField.bindingPath = Info.SettingsPropertyPath;
            _settingsField.RegisterValueChangedCallback(OnSettingsObjectChanged);
            settingsAssetContainer.Add(_settingsField);

            // Create Button
            Button createSettingsAssetButton = new Button(CreateSettingsAsset)
            {
                name = "CreateSettingsAssetButton",
                text = "New",
            };
            settingsAssetContainer.Add(createSettingsAssetButton);

            #endregion


            // Execution Button
            _executeButton = new Button(ExecuteChecker)
            {
                name = "ExecuteButton",
                text = "Execute",
                style =
                {
                    marginLeft = 8,
                    marginRight = 8,
                    marginTop = 8,
                    marginBottom = 8,
                }
            };
            _executeButton.SetEnabled(_settings);
            root.Add(_executeButton);

            // Separator
            root.Add(new VisualElement
            {
                name = "SettingsSeparator",
                style =
                {
                    backgroundColor = EditorGUIUtility.isProSkin
                        ? new Color(26 / 255f, 26 / 255f, 26 / 255f, 1.0f)
                        : new Color(127 / 255f, 127 / 255f, 127 / 255f, 1.0f),
                    width = Length.Percent(100),
                    height = 1,
                    minHeight = 1,
                    maxHeight = 1,
                    marginLeft = 16,
                    marginRight = 16,
                }
            });

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
            ListView resultListView = new ListView
            {
                name = "ResultListView",
            };
            resultContainer.Pane1.Add(resultListView);

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

            // Details Scroll View
            ScrollView detailsScrollView = new ScrollView()
            {
                name = "DetailsScrollView",
            };
            resultContainer.Pane2.Add(detailsScrollView);

            #endregion


            // Bind properties
            root.Bind(new SerializedObject(this));

            // Restore values
            // _settingsField.SetValueWithoutNotify(_settings);
        }
    }
}