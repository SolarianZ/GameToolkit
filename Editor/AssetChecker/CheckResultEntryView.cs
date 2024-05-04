using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public enum ResultIconStyle
    {
        Style1,
        Style2,
        Style3
    }

    public class CheckResultEntryView : VisualElement
    {
        private readonly Image _typeImage;
        private readonly Label _label;
        private readonly Image _repairableImage;
        private ResultType _resultType;
        internal ResultIconStyle IconStyle { get; set; }


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
                tooltip = "Repairable",
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
            _resultType = result.type;
            UpdateTypeIcon();

            _label.text = result.title;
            _repairableImage.style.display = result.repairable
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        public void SetIconStyle(ResultIconStyle iconStyle)
        {
            IconStyle = iconStyle;
            UpdateTypeIcon();
        }

        private void UpdateTypeIcon()
        {
            switch (_resultType)
            {
                case ResultType.AllPass:
                    switch (IconStyle)
                    {
                        case ResultIconStyle.Style1:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style2:
                            _typeImage.image = EditorGUIUtility.IconContent("d_winbtn_mac_max@2x").image;
                            break;
                        case ResultIconStyle.Style3:
                            _typeImage.image = EditorGUIUtility.IconContent("d_greenLight").image;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(IconStyle), IconStyle, null);
                    }
                    break;
                case ResultType.NotImportant:
                    switch (IconStyle)
                    {
                        case ResultIconStyle.Style1:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot0_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style2:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot0_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style3:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot0_pix16_gizmo").image;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(IconStyle), IconStyle, null);
                    }
                    break;
                case ResultType.Warning:
                    switch (IconStyle)
                    {
                        case ResultIconStyle.Style1:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot5_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style2:
                            _typeImage.image = EditorGUIUtility.IconContent("d_winbtn_mac_min@2x").image;
                            break;
                        case ResultIconStyle.Style3:
                            _typeImage.image = EditorGUIUtility.IconContent("d_orangeLight").image;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(IconStyle), IconStyle, null);
                    }
                    break;
                case ResultType.Error:
                    switch (IconStyle)
                    {
                        case ResultIconStyle.Style1:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot6_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style2:
                            _typeImage.image = EditorGUIUtility.IconContent("d_winbtn_mac_close@2x").image;
                            break;
                        case ResultIconStyle.Style3:
                            _typeImage.image = EditorGUIUtility.IconContent("d_redLight").image;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(IconStyle), IconStyle, null);
                    }
                    break;
                case ResultType.Exception:
                    switch (IconStyle)
                    {
                        case ResultIconStyle.Style1:
                            _typeImage.image = EditorGUIUtility.IconContent("sv_icon_dot7_pix16_gizmo").image;
                            break;
                        case ResultIconStyle.Style2:
                            _typeImage.image = EditorGUIUtility.IconContent("d_winbtn_mac_close_a@2x").image;
                            break;
                        case ResultIconStyle.Style3:
                            _typeImage.image = EditorGUIUtility.IconContent("Error@2x").image;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(IconStyle), IconStyle, null);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_resultType), _resultType, null);
            }
        }
    }
}