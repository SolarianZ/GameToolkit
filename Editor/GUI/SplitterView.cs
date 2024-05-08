using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.GUI
{
    [Obsolete("Use UnityEngine.UIElements.TwoPaneSplitView instead.")]
    public class SplitterView : VisualElement
    {
        public VisualElement Pane1 { get; }
        public VisualElement Pane2 { get; }
        public VisualElement Splitter { get; }


        public SplitterView(FlexDirection flexDirection, float splitterThickness = 2)
        {
            style.flexGrow = 1;
            style.flexDirection = flexDirection;

            // Pane1
            Pane1 = new VisualElement
            {
                name = "Pane1",
                style =
                {
                    flexGrow = 0,
                    paddingLeft = 2,
                    paddingRight = 2,
                    paddingTop = 2,
                    paddingBottom = 2,
                }
            };
            switch (flexDirection)
            {
                case FlexDirection.Column:
                case FlexDirection.ColumnReverse:
                    Pane1.style.height = Length.Percent(50);
                    break;
                case FlexDirection.Row:
                case FlexDirection.RowReverse:
                    Pane1.style.width = Length.Percent(50);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flexDirection), flexDirection, null);
            }

            Add(Pane1);

            // Pane2
            Pane2 = new VisualElement
            {
                name = "Pane2",
                style =
                {
                    flexGrow = 1,
                    paddingLeft = 2,
                    paddingRight = 2,
                    paddingTop = 2,
                    paddingBottom = 2,
                }
            };
            Add(Pane2);

            // Splitter
            Splitter = CreateSplitter(flexDirection, splitterThickness);
            Splitter.AddManipulator(new SplitterDragManipulator(Pane1, Pane2));
            Insert(1, Splitter);
        }

        private static VisualElement CreateSplitter(FlexDirection flexDirection, float splitterThickness)
        {
            VisualElement splitter = new VisualElement
            {
                name = "Splitter",
                style =
                {
                    backgroundColor = EditorGUIUtility.isProSkin
                        ? new Color(26 / 255f, 26 / 255f, 26 / 255f, 1.0f)
                        : new Color(127 / 255f, 127 / 255f, 127 / 255f, 1.0f),
                }
            };

            switch (flexDirection)
            {
                case FlexDirection.Column:
                case FlexDirection.ColumnReverse:
                    splitter.style.width = Length.Percent(100);
                    splitter.style.height = splitterThickness;
                    splitter.style.cursor = SplitterDragManipulator.LoadCursor(MouseCursor.SplitResizeUpDown);
                    break;
                case FlexDirection.Row:
                case FlexDirection.RowReverse:
                    splitter.style.width = splitterThickness;
                    splitter.style.height = Length.Percent(100);
                    splitter.style.cursor = SplitterDragManipulator.LoadCursor(MouseCursor.SplitResizeLeftRight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flexDirection), flexDirection, null);
            }

            return splitter;
        }
    }
}