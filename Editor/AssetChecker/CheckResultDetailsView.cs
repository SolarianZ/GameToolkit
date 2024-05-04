using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public class CheckResultDetailsView : VisualElement
    {
        private readonly Label _titleLabel;
        private readonly ObjectView _assetView;
        private readonly ObjectView _checkerView;
        private readonly Label _detailsLabel;
        private readonly Button _recheckButton;
        private readonly Button _repairButton;
        private readonly IList<AssetCheckResult> _results;
        private int _selectionIndex;

        public event Action<int> AssetRechecked;
        public event Action<int, bool> AssetRepaired;


        public CheckResultDetailsView(IList<AssetCheckResult> results)
        {
            _results = results;
            style.flexGrow = 1;

            const float ButtonHeight = 28;

            _titleLabel = new Label
            {
                name = "TitleLabel",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                    marginTop = 4,
                    marginBottom = 4,
                }
            };
            Add(_titleLabel);

            _assetView = new ObjectView("Asset")
            {
                name = "AssetView",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                }
            };
            Add(_assetView);

            _checkerView = new ObjectView("Checker")
            {
                name = "CheckerView",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                }
            };
            Add(_checkerView);

            ScrollView detailsScrollView = new ScrollView
            {
                name = "DetailsScrollView",
                style =
                {
                    flexGrow = 1,
                }
            };
            Add(detailsScrollView);

            _detailsLabel = new Label
            {
                name = "SelectableDetailsLabel",
                enableRichText = true,
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    marginLeft = 4,
                    marginRight = 4,
                    marginTop = 4,
                    marginBottom = 4,
                }
            };
            ((ITextSelection)_detailsLabel).isSelectable = true;
            detailsScrollView.contentContainer.Add(_detailsLabel);

            VisualElement buttonContainer = new VisualElement
            {
                name = "ButtonContainer",
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.FlexEnd,
                    height = ButtonHeight,
                    minHeight = ButtonHeight,
                    maxHeight = ButtonHeight,
                    marginLeft = 4,
                    marginRight = 4,
                    marginTop = 4,
                    marginBottom = 4,
                    paddingRight = 8,
                },
            };
            Add(buttonContainer);

            _recheckButton = new Button(RecheckAsset)
            {
                name = "RecheckButton",
                text = "Recheck",
            };
            _recheckButton.SetEnabled(false);
            buttonContainer.Add(_recheckButton);

            _repairButton = new Button(RepairAsset)
            {
                name = "RepairButton",
                text = "Try Repair",
            };
            _repairButton.SetEnabled(false);
            buttonContainer.Add(_repairButton);
        }

        public void SelectResult(int index)
        {
            _selectionIndex = index;
            if (_selectionIndex == -1)
            {
                ClearSelection();
                return;
            }

            AssetCheckResult result = _results[_selectionIndex];
            if (result == null)
            {
                ClearSelection();
                return;
            }

            _titleLabel.text = result.title;
            _assetView.SetAsset(result.asset);
            _checkerView.SetAsset(result.checker);
            _detailsLabel.text = result.details;
            _recheckButton.SetEnabled(result.asset && result.checker);
            _repairButton.SetEnabled(result.repairable);
        }

        public void ClearSelection()
        {
            _selectionIndex = -1;

            _titleLabel.text = "-";
            _detailsLabel.text = "-";
            _assetView.SetAsset(null);
            _checkerView.SetAsset(null);
            _recheckButton.SetEnabled(false);
            _repairButton.SetEnabled(false);
        }

        private void RecheckAsset()
        {
            AssetCheckResult result = _results[_selectionIndex];
            try
            {
                AssetCheckResult newResult = result.checker.CheckAsset(result.asset);
                _results[_selectionIndex] = newResult;
            }
            catch (Exception e)
            {
                result.type = ResultType.Exception;
                result.title = e.GetType().Name;
                result.details = e.Message;
                result.repairable = false;
            }

            SelectResult(_selectionIndex);

            AssetRechecked?.Invoke(_selectionIndex);
        }

        private void RepairAsset()
        {
            AssetCheckResult result = _results[_selectionIndex];
            try
            {
                result.checker.RepairAsset(result, out bool allIssuesRepaired);
                if (allIssuesRepaired)
                {
                    AssetRepaired?.Invoke(_selectionIndex, true);
                }
                else
                {
                    AssetRepaired?.Invoke(_selectionIndex, false);
                }
            }
            catch (Exception e)
            {
                result.type = ResultType.Exception;
                result.title = e.GetType().Name;
                result.details = e.Message;
                result.repairable = false;

                SelectResult(_selectionIndex);

                AssetRepaired?.Invoke(_selectionIndex, false);
            }
        }


        public class ObjectView : VisualElement
        {
            private readonly ObjectField _assetField;
            private readonly Button _pingAssetButton;

            public ObjectView(string label)
            {
                style.flexDirection = FlexDirection.Row;

                Label assetLabel = new Label
                {
                    name = "AssetLabel",
                    text = label,
                    style =
                    {
                        width = 60,
                        minWidth = 60,
                        maxWidth = 60,
                    },
                };
                Add(assetLabel);

                _assetField = new ObjectField
                {
                    name = "AssetField",
                    style =
                    {
                        flexGrow = 1,
                    }
                };
                _assetField.SetEnabled(false);
                Add(_assetField);

                _pingAssetButton = new Button(PingAsset)
                {
                    name = "PingAssetButton",
                    text = "Ping",
                };
                _pingAssetButton.SetEnabled(false);
                Add(_pingAssetButton);
            }

            public void SetAsset(UObject asset)
            {
                _assetField.value = asset;
                _pingAssetButton.SetEnabled(asset);
            }

            private void PingAsset()
            {
                if (_assetField.value)
                {
                    EditorGUIUtility.PingObject(_assetField.value);
                }
            }
        }
    }
}