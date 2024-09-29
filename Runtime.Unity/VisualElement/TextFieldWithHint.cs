using UnityEngine;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.UIElements
{
    public class TextFieldWithHint : TextField
    {
        public static readonly string HintLabelUssClassName = ussClassName + "__hint-label";

        public string HintText
        {
            get => HintLabel.text;
            set => HintLabel.text = value;
        }

        public Label HintLabel { get; }

        public override string value
        {
            get => base.value;
            set
            {
                base.value = value;
                RefreshHintDisplay();
            }
        }


        public TextFieldWithHint()
        {
            HintLabel = new Label()
            {
                name = "hint-label",
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position = Position.Absolute,
                    left = 0,
                    right = 0,
                    unityFontStyleAndWeight = FontStyle.Italic,
                }
            };
            HintLabel.AddToClassList(HintLabelUssClassName);

            string textElementClassName =
#if UNITY_2022_1_OR_NEWER
                TextInputBase.innerTextElementUssClassName;
#else
                inputUssClassName;
#endif
            VisualElement textElement = this.Q<VisualElement>(className: textElementClassName);
            textElement.Add(HintLabel);
        }

        public void RefreshHintDisplay()
        {
            if (HintLabel != null)
            {
                HintLabel.style.display = string.IsNullOrEmpty(value)
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }
        }

        public override void SetValueWithoutNotify(string newValue)
        {
            base.SetValueWithoutNotify(newValue);
            RefreshHintDisplay();
        }
    }
}
