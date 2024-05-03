using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public class CheckResultEntryView : VisualElement
    {
        private readonly Image _typeImage;
        private readonly Label _label;
        private readonly Image _repairableImage;


        public CheckResultEntryView()
        {
            style.flexDirection = FlexDirection.Row;

            _typeImage = new Image
            {
                name = "TypeImage",
                style =
                {
                    width = 20,
                    minWidth = 20,
                    maxWidth = 20,
                    height = 20,
                    minHeight = 20,
                    maxHeight = 20,
                    alignSelf = Align.Center,
                }
            };
            Add(_typeImage);

            _label = new Label
            {
                name = "Label",
                style =
                {
                    flexGrow = 1,
                    flexShrink = 1,
                    overflow = Overflow.Hidden,
                    unityTextAlign = TextAnchor.MiddleLeft,
                }
            };
            Add(_label);

            _repairableImage = new Image
            {
                name = "RepairableImage",
                image = EditorGUIUtility.isProSkin
                            ? EditorGUIUtility.IconContent("d_CustomTool@2x").image
                            : EditorGUIUtility.IconContent("CustomTool@2x").image,
                style =
                {
                    width = 20,
                    minWidth = 20,
                    maxWidth = 20,
                    height = 20,
                    minHeight = 20,
                    maxHeight = 20,
                    alignSelf = Align.Center,
                }
            };
            Add(_repairableImage);
        }

        public void Bind(AssetCheckResult result)
        {
            switch (result.type)
            {
                case ResultType.NotImportant:
                    _typeImage.image = EditorGUIUtility.isProSkin
                        ? EditorGUIUtility.IconContent("d_console.infoicon.sml@2x").image
                        : EditorGUIUtility.IconContent("console.infoicon.sml@2x").image;
                    break;
                case ResultType.Warning:
                    _typeImage.image = EditorGUIUtility.IconContent("Warning@2x").image;
                    break;
                case ResultType.Error:
                    _typeImage.image = EditorGUIUtility.IconContent("Error@2x").image;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.type), result.type, null);
            }

            _label.text = result.title;
            _repairableImage.style.display = result.repairable
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}