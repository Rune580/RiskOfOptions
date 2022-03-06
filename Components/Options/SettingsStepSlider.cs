using System;
using System.Globalization;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.OptionComponents
{
    public class SettingsStepSlider : BaseSettingsControl
    {
        public Slider slider;

        public HGTextMeshProUGUI valueText;

        public float minValue;

        public float maxValue;

        public float internalMinValue;

        public float internalMaxValue;

        public float increment;

        public string formatString = "{0:0.00}";

        protected new void Awake()
        {
            if (!slider)
                slider = transform.Find("SliderControl").Find("Slider").GetComponent<Slider>();

            if (!valueText)
                valueText = transform.Find("SliderControl").Find("TextArea").GetComponentInChildren<HGTextMeshProUGUI>();

            if (!nameLabel)
                nameLabel = GetComponent<LanguageTextMeshController>();

            base.Awake();

            double stepsHighAccuracy = (Math.Abs(internalMinValue - internalMaxValue) / increment);

            int steps = (int)Math.Round(stepsHighAccuracy);

            slider.minValue = 0;
            slider.maxValue = steps;

            slider.wholeNumbers = true;

            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float newValue)
        {
            if (inUpdateControls)
                return;

            float remapValue = (newValue * increment) + internalMinValue;

            SubmitSetting(TextSerialization.ToStringInvariant(remapValue));

            UpdateControls();
        }

        private void OnInputFieldSubmit(string newString)
        {
            if (inUpdateControls)
                return;

            if (TextSerialization.TryParseInvariant(newString, out float value))
            {
                value = value.Remap(minValue, maxValue, internalMinValue, internalMaxValue);
                SubmitSetting(TextSerialization.ToStringInvariant(value));
            }
                
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            if (!TextSerialization.TryParseInvariant(GetCurrentValue(), out float value))
                return;

            float num = Mathf.Clamp(value, internalMinValue, internalMaxValue);
            if (slider)
            {
                slider.value = Math.Abs(num - internalMinValue) / increment;
            }
            if (valueText)
            {
                valueText.text = num.ToString(CultureInfo.InvariantCulture);
                //string.Format(CultureInfo.InvariantCulture, formatString, num);
            }
        }

        public void MoveSlider(float delta)
        {
            if (slider)
                slider.normalizedValue += delta;
        }
    }
}
