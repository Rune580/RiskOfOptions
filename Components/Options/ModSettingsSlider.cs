using System.Globalization;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsSlider : ModSettingsControl
    {
        public Slider slider;
        public HGTextMeshProUGUI valueText;
        public float minValue;
        public float maxValue;
        public string formatString = "{0:0.00}%";

        protected new void Awake()
        {
            base.Awake();
            
            if (!slider)
                return;
            
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float newValue)
        {
            if (InUpdateControls)
                return;
            
            SubmitSetting(TextSerialization.ToStringInvariant(newValue));
        }
        
        private void OnInputFieldSubmit(string newString)
        {
            if (InUpdateControls)
                return;
            
            if (TextSerialization.TryParseInvariant(newString, out float value))
                SubmitSetting(TextSerialization.ToStringInvariant(value));
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            if (!TextSerialization.TryParseInvariant(GetCurrentValue(), out float value))
                return;
            
            float num = Mathf.Clamp(value, minValue, maxValue);

            if (slider)
                slider.value = num;

            if (valueText)
                valueText.text = string.Format(CultureInfo.InvariantCulture, formatString, num);
        }

        public void MoveSlider(float delta)
        {
            if (slider)
                slider.normalizedValue += delta;
        }
    }
}