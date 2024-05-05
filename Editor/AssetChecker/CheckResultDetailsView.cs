using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public delegate void AssetRecheckedHandler(AssetCheckResult newResult, AssetCheckResult oldResult);
    public delegate void AssetRepairedHandler(AssetCheckResult result, bool allIssuesRepaired);

    public class CheckResultDetailsView : VisualElement
    {
        private readonly Label _typeLabel;
        private readonly Label _titleLabel;
        private readonly ObjectView _assetView;
        private readonly ObjectView _checkerView;
        private readonly Label _detailsLabel;
        private readonly Button _recheckButton;
        private readonly Button _repairButton;
        private AssetCheckResult _selectedResult;

        public event AssetRecheckedHandler AssetRechecked;
        public event AssetRepairedHandler AssetRepaired;


        public CheckResultDetailsView()
        {
            style.flexGrow = 1;

            VisualElement labelContainer = new VisualElement
            {
                name = "LabelContainer",
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginLeft = 4,
                    marginRight = 4,
                    marginTop = 4,
                    marginBottom = 4,
                    overflow = Overflow.Hidden,
                }
            };
            Add(labelContainer);

            _typeLabel = new Label
            {
                name = "TypeLabel",
                text = "-",
                style =
                {
                    paddingLeft= 2,
                    paddingRight= 2,
                    borderLeftWidth = 2,
                    borderRightWidth = 2,
                    borderTopWidth = 2,
                    borderBottomWidth = 2,
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            labelContainer.Add(_typeLabel);

            _titleLabel = new Label
            {
                name = "TitleLabel",
                style =
                {
                    marginLeft = 4,
                    unityTextAlign = TextAnchor.MiddleLeft,
                }
            };
            labelContainer.Add(_titleLabel);

            _assetView = new ObjectView(this, false)
            {
                name = "AssetView",
                style =
                {
                    marginLeft = 4,
                    marginRight = 4,
                }
            };
            Add(_assetView);

            _checkerView = new ObjectView(this, true)
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
                    marginTop = 8,
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

            const float ButtonHeight = 28;
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

        public void SelectResult(AssetCheckResult selectedResult)
        {
            _selectedResult = selectedResult;
            if (_selectedResult == null)
            {
                ClearSelection();
                return;
            }

            UpdateResultTypeBorderColor(_selectedResult.type);
            _typeLabel.text = ObjectNames.NicifyVariableName(_selectedResult.type.ToString());
            _titleLabel.text = _selectedResult.title;
            _assetView.UpdateView();
            _checkerView.UpdateView();
            _detailsLabel.text = _selectedResult.details;
            _recheckButton.SetEnabled(_selectedResult.asset && _selectedResult.checker);
            _repairButton.SetEnabled(_selectedResult.repairable);
        }

        public void ClearSelection()
        {
            _selectedResult = null;

            UpdateResultTypeBorderColor(CheckResultType.NotImportant);
            _typeLabel.text = "-";
            _titleLabel.text = "-";
            _detailsLabel.text = "-";
            _assetView.UpdateView();
            _checkerView.UpdateView();
            _recheckButton.SetEnabled(false);
            _repairButton.SetEnabled(false);
        }

        private void UpdateResultTypeBorderColor(CheckResultType resultType)
        {
            Color color = resultType.GetResultTypeBorderColor();
            _typeLabel.style.borderLeftColor = color;
            _typeLabel.style.borderRightColor = color;
            _typeLabel.style.borderTopColor = color;
            _typeLabel.style.borderBottomColor = color;
        }

        private void RecheckAsset()
        {
            AssetCheckResult oldResult = _selectedResult;
            AssetCheckResult newResult;
            try
            {
                newResult = oldResult.checker.CheckAsset(oldResult.asset);
            }
            catch (Exception e)
            {
                newResult = new AssetCheckResult
                {
                    type = CheckResultType.Exception,
                    title = e.GetType().Name,
                    details = e.Message,
                    asset = oldResult.asset,
                    checker = oldResult.checker,
                    repairable = false,
                };
            }

            SelectResult(newResult);

            AssetRechecked?.Invoke(newResult, oldResult);
        }

        private void RepairAsset()
        {
            try
            {
                _selectedResult.checker.RepairAsset(_selectedResult, out bool allIssuesRepaired);
                if (allIssuesRepaired)
                {
                    AssetRepaired?.Invoke(_selectedResult, true);
                }
                else
                {
                    AssetRepaired?.Invoke(_selectedResult, false);
                }
            }
            catch (Exception e)
            {
                _selectedResult.type = CheckResultType.Exception;
                _selectedResult.title = e.GetType().Name;
                _selectedResult.details = e.Message;
                _selectedResult.repairable = false;

                SelectResult(_selectedResult);

                AssetRepaired?.Invoke(_selectedResult, false);
            }
        }


        public class ObjectView : VisualElement
        {
            private readonly Image _objectIconImage;
            private readonly Label _objectPathLabel;
            private readonly Button _pingAssetButton;
            private readonly CheckResultDetailsView _owner;
            private readonly bool _isCheckerView;


            public ObjectView(CheckResultDetailsView owner, bool isCheckerView)
            {
                _owner = owner;
                _isCheckerView = isCheckerView;
                style.flexDirection = FlexDirection.Row;

                Label objectLabel = new Label
                {
                    name = "ObjectLabel",
                    text = isCheckerView ? "Checker" : "Asset",
                    style =
                    {
                        width = 52,
                        minWidth = 52,
                        maxWidth = 52,
                        paddingRight = 4,
                        unityTextAlign = TextAnchor.MiddleRight,
                    },
                };
                Add(objectLabel);

                const float ObjectIconSize = 20;
                _objectIconImage = new Image
                {
                    name = "ObjectIconImage",
                    style =
                    {
                        width = ObjectIconSize,
                        minWidth = ObjectIconSize,
                        maxWidth = ObjectIconSize,
                        height = ObjectIconSize,
                        minHeight = ObjectIconSize,
                        maxHeight = ObjectIconSize,
                    }
                };
                Add(_objectIconImage);

                _objectPathLabel = new Label
                {
                    name = "ObjectPathLabel",
                    text = "-",
                    style =
                    {
                        flexGrow = 1,
                        flexShrink = 1,
                        marginLeft = 4,
                        overflow = Overflow.Hidden,
                        unityFontStyleAndWeight = FontStyle.Italic,
                        unityTextAlign = TextAnchor.MiddleLeft,
                    }
                };
                ((ITextSelection)_objectPathLabel).isSelectable = true;
                Add(_objectPathLabel);

                _pingAssetButton = new Button(PingAsset)
                {
                    name = "PingAssetButton",
                    text = "Ping",
                };
                _pingAssetButton.SetEnabled(false);
                Add(_pingAssetButton);
            }

            public void UpdateView()
            {
                UObject target = null;
                if (_owner._selectedResult != null)
                {
                    target = _isCheckerView
                        ? _owner._selectedResult.checker
                        : _owner._selectedResult.asset;
                }

                _pingAssetButton.SetEnabled(target);

                if (target)
                {
                    string targetPath = AssetDatabase.GetAssetPath(target);
                    _objectPathLabel.text = targetPath;
                    _objectIconImage.image = AssetDatabase.GetCachedIcon(targetPath);
                }
                else
                {
                    _objectPathLabel.text = "-";
                    _objectIconImage.image = EditorGUIUtility.isProSkin
                        ? EditorGUIUtility.IconContent("d_GameObject Icon").image
                        : EditorGUIUtility.IconContent("GameObject Icon").image;
                }
            }

            private void PingAsset()
            {
                if (_owner._selectedResult != null)
                {
                    UObject target = _isCheckerView
                        ? _owner._selectedResult.checker
                        : _owner._selectedResult.asset;
                    if (target)
                    {
                        EditorGUIUtility.PingObject(target);
                    }
                }
            }
        }
    }
}