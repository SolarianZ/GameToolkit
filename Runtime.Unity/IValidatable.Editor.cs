#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor
{
    public abstract class ValidatableEditor : UnityEditor.Editor
    {
        protected virtual List<ValidationResult> ValidationResults => EditorValidationUtility.SharedValidationResults;
        protected virtual double ValidationInterval => EditorValidationUtility.SharedValidationInterval;
        protected double PrevValidationTime { get; set; }
        protected abstract ListView ValidationResultListView { get; set; }


        protected virtual void OnEnable()
        {
            PrevValidationTime = 0;
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        protected virtual void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
        }

        protected virtual void OnUpdate()
        {
            if (EditorApplication.timeSinceStartup - PrevValidationTime < ValidationInterval)
            {
                return;
            }

            PrevValidationTime = EditorApplication.timeSinceStartup;
            ValidationResultListView?.UpdateValidation((IValidatable)target, ValidationResults);
        }
    }

    public static class EditorValidationUtility
    {
        public static readonly List<ValidationResult> SharedValidationResults = new();
        public static double SharedValidationInterval { get; set; } = 0.25;


        public static void DrawHelpBox(this ValidationResult result, bool wide = true)
        {
            EditorGUILayout.HelpBox(result.Content, (MessageType)result.Type, wide);
        }


        public static ScrollView CreateValidationResultScrollView()
        {
            ScrollView validationResultScroll = new ScrollView
            {
                name = "Message ScrollView",
                style =
                {
                    maxHeight = 300,
                }
            };

            return validationResultScroll;
        }

        public static ListView CreateValidationResultList(IList itemSource,
            Func<VisualElement> makeItem, Action<VisualElement, int> bindItem,
            Action<VisualElement, int> unbindItem, float fixedItemHeight = 50)
        {
            ListView validationResultList = new ListView
            {
                name = "Message List",
                itemsSource = itemSource,
                makeItem = makeItem,
                bindItem = bindItem,
                unbindItem = unbindItem,
                fixedItemHeight = fixedItemHeight,
                showAddRemoveFooter = false,
                showBorder = false,
                reorderable = false,
                selectionType = SelectionType.Single,
            };

            return validationResultList;
        }

        public static ListView CreateSharedValidationResultListView(float fixedItemHeight = 50)
        {
            return CreateValidationResultList(SharedValidationResults, MakeValidationResultView,
                BindSharedValidationResultView, UnbindValidationResultView, fixedItemHeight);
        }

        public static VisualElement MakeValidationResultView()
        {
            HelpBox helpBox = new HelpBox
            {
                style =
                {
                    paddingBottom = 0,
                }
            };
            return helpBox;
        }

        public static void BindSharedValidationResultView(VisualElement helpBoxElement, int index)
        {
            HelpBox helpBox = (HelpBox)helpBoxElement;
            ValidationResult result = SharedValidationResults[index];
            helpBox.text = result.Content;
            helpBox.messageType = (HelpBoxMessageType)result.Type;
            helpBox.userData = result.Context;
        }

        public static void UnbindValidationResultView(VisualElement helpBoxElement, int index)
        {
            HelpBox helpBox = (HelpBox)helpBoxElement;
            helpBox.userData = null;
        }

        public static void UpdateValidation(this ListView validationResultListView,
            IValidatable targetValidatable, List<ValidationResult> validationResults)
        {
            validationResults.Clear();
            targetValidatable.Validate(validationResults);

            validationResultListView.RefreshItems();
            if (validationResultListView.itemsSource.Count == 0)
            {
                validationResultListView.Q<Label>(className: "unity-list-view__empty-label").style.display = DisplayStyle.None;
            }
        }

        public static void UpdateSharedValidation(this ListView validationResultListView,
            IValidatable targetValidatable)
        {
            validationResultListView.UpdateValidation(targetValidatable, SharedValidationResults);
        }
    }
}
#endif
