using System.Globalization;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsIntSlider : ModSettingsControl<int>
    {
        public Slider slider;
        public HGTextMeshProUGUI valueText;

        public int minValue;
        public int maxValue;
        public string formatString;

        protected override void Awake()
        {
            base.Awake();

            if (!slider)
                return;

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = true;
            
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
            
            SubmitValue(Mathf.RoundToInt(newValue));
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            int num = Mathf.Clamp(GetCurrentValue(), minValue, maxValue);

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