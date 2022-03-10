using System;
using System.Globalization;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsStepSlider : ModSettingsControl<float>
    {
        public Slider slider;
        public HGTextMeshProUGUI valueText;
        
        public float minValue;
        public float maxValue;
        public float increment;
        public string formatString;

        protected override void Awake()
        {
            base.Awake();
            
            if (!slider)
                return;
            
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
        protected override void Disable()
        {
            slider.interactable = false;
            
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = slider.colors.disabledColor;
            slider.transform.parent.Find("TextArea").GetComponent<Image>().color = slider.colors.disabledColor;
            
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = false;
        }

        protected override void Enable()
        {
            slider.interactable = true;
            
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = slider.colors.normalColor;
            slider.transform.parent.Find("TextArea").GetComponent<Image>().color = GetComponent<HGButton>().colors.normalColor;
            
            foreach (var button in GetComponentsInChildren<HGButton>())
                button.interactable = true;
        }

        private void OnSliderValueChanged(float newValue)
        {
            if (InUpdateControls)
                return;

            float remapValue = (newValue * increment) + minValue;

            SubmitValue(remapValue);
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            float num = Mathf.Clamp(GetCurrentValue(), minValue, maxValue);
            
            if (slider)
                slider.value = Math.Abs(num - minValue) / increment;

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
