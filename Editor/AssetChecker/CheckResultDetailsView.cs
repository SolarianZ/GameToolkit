using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public class CheckResultDetailsView : VisualElement
    {
        private readonly Label _titleLabel;
        private readonly ObjectField _assetField;
        private readonly ObjectField _checkerField;
        private readonly Label _detailsLabel;
        private readonly Button _recheckButton;
        private readonly Button _repairButton;
        private readonly IList<AssetCheckResult> _results;
        private int _selectionIndex;

        public event Action<int> AssetRechecked;
        public event Action<int> AssetRepaired;


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

            _assetField = new ObjectField
            {
                name = "AssetField",
                label = "Asset",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                }
            };
            _assetField.SetEnabled(false);
            Add(_assetField);

            _checkerField = new ObjectField
            {
                name = "CheckerField",
                label = "Checker",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                }
            };
            _checkerField.SetEnabled(false);
            Add(_checkerField);

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
                name = "DetailsLabel",
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    marginLeft = 4,
                    marginRight = 4,
                    marginTop = 4,
                    marginBottom = 4,
                }
            };
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
            _titleLabel.text = result.title;
            _assetField.value = result.asset;
            _checkerField.value = result.checker;
            _detailsLabel.text = result.details;
            _recheckButton.SetEnabled(result.asset && result.checker);
            _repairButton.SetEnabled(result.repairable);
        }

        public void ClearSelection()
        {
            _selectionIndex = -1;

            _titleLabel.text = "-";
            _detailsLabel.text = "-";
            _assetField.value = null;
            _checkerField.value = null;
            _recheckButton.SetEnabled(false);
            _repairButton.SetEnabled(false);
        }

        private void RecheckAsset()
        {
            AssetCheckResult result = _results[_selectionIndex];
            AssetCheckResult newResult = result.checker.CheckAsset(result.asset);
            _results[_selectionIndex] = newResult;
            SelectResult(_selectionIndex);

            AssetRechecked?.Invoke(_selectionIndex);
        }

        private void RepairAsset()
        {
            AssetCheckResult result = _results[_selectionIndex];
            if (result.checker.TryRepairAsset(result))
            {
                int repairedIndex = _selectionIndex;
                ClearSelection();

                AssetRepaired?.Invoke(repairedIndex);
            }
        }
    }
}