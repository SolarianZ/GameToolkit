using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UICursor = UnityEngine.UIElements.Cursor;

namespace GBG.GameToolkit.Unity.Editor.GUI
{
    public class SplitterDragManipulator : MouseManipulator
    {
        #region Static

        public static UICursor LoadCursor(MouseCursor cursorStyle)
        {
            object boxed = new UICursor();
            typeof(UICursor).GetProperty("defaultCursorId", BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.SetValue(boxed, (int)cursorStyle, null);

            return (UICursor)boxed;
        }

        #endregion


        private readonly VisualElement _pane1;
        private readonly VisualElement _pane2;
        private bool _isActive;
        private Vector2 _start;
        private float _pane1OriginalSize;


        public SplitterDragManipulator(VisualElement firstPane, VisualElement secondPane)
        {
            _pane1 = firstPane;
            _pane2 = secondPane;

            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse,
            });
        }


        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }


        private void OnMouseDown(MouseDownEvent e)
        {
            if (_isActive)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                _isActive = true;
                _start = e.mousePosition;

                IResolvedStyle containerResolvedStyle = target.parent.resolvedStyle;
                FlexDirection direction = containerResolvedStyle.flexDirection;
                switch (direction)
                {
                    case FlexDirection.Column:
                    case FlexDirection.ColumnReverse:
                        _pane1OriginalSize = _pane1.resolvedStyle.height;
                        break;
                    case FlexDirection.Row:
                    case FlexDirection.RowReverse:
                        _pane1OriginalSize = _pane1.resolvedStyle.width;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent e)
        {
            if (!_isActive || !target.HasMouseCapture())
            {
                return;
            }

            IResolvedStyle containerResolvedStyle = target.parent.resolvedStyle;
            FlexDirection direction = containerResolvedStyle.flexDirection;
            Vector2 mouseDelta = e.mousePosition - _start;
            float sizeDelta;
            switch (direction)
            {
                case FlexDirection.Column:
                    sizeDelta = mouseDelta.y;
                    break;
                case FlexDirection.ColumnReverse:
                    sizeDelta = -mouseDelta.y;
                    break;
                case FlexDirection.Row:
                    sizeDelta = mouseDelta.x;
                    break;
                case FlexDirection.RowReverse:
                    sizeDelta = -mouseDelta.x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            ApplyDelta(sizeDelta);
            e.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            if (!_isActive || !target.HasMouseCapture() || !CanStopManipulation(e))
            {
                return;
            }

            _isActive = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }

        private void ApplyDelta(float delta)
        {
            float pane1Size = _pane1OriginalSize + delta;

            IResolvedStyle containerResolvedStyle = target.parent.resolvedStyle;
            FlexDirection direction = containerResolvedStyle.flexDirection;
            switch (direction)
            {
                case FlexDirection.Column:
                case FlexDirection.ColumnReverse:
                {
                    float containerSize = containerResolvedStyle.height;
                    float pane1SizeRatio = pane1Size / containerSize;
                    float pane2SizeRatio = 1 - pane1SizeRatio;
                    float pane2Height = pane2SizeRatio * containerSize;
                    if (CheckHeight(_pane2, pane2Height))
                    {
                        _pane1.style.height = Length.Percent(pane1Size / containerSize * 100);
                    }

                    break;
                }
                case FlexDirection.Row:
                case FlexDirection.RowReverse:
                {
                    float containerSize = containerResolvedStyle.width;
                    float pane1SizeRatio = pane1Size / containerSize;
                    float pane2SizeRatio = 1 - pane1SizeRatio;
                    float pane2Width = pane2SizeRatio * containerSize;
                    if (CheckWidth(_pane2, pane2Width))
                    {
                        _pane1.style.width = Length.Percent(pane1Size / containerSize * 100);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private bool CheckWidth(VisualElement element, float width)
        {
            float minSize = float.MinValue;
            if (element.resolvedStyle.minWidth.keyword == StyleKeyword.Undefined)
            {
                minSize = element.resolvedStyle.minWidth.value;
            }

            float maxSize = float.MaxValue;
            if (element.resolvedStyle.maxWidth.keyword == StyleKeyword.Undefined)
            {
                maxSize = element.resolvedStyle.maxWidth.value;
            }

            return minSize <= width && width <= maxSize;
        }

        private bool CheckHeight(VisualElement element, float height)
        {
            float minSize = float.MinValue;
            if (element.resolvedStyle.minHeight.keyword == StyleKeyword.Undefined)
            {
                minSize = element.resolvedStyle.minHeight.value;
            }

            float maxSize = float.MaxValue;
            if (element.resolvedStyle.maxHeight.keyword == StyleKeyword.Undefined)
            {
                maxSize = element.resolvedStyle.maxHeight.value;
            }

            return minSize <= height && height <= maxSize;
        }
    }
}