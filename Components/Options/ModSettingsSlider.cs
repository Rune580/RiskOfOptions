using System.Globalization;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options
{
    public class ModSettingsSlider : ModSettingsControl<float>
    {
        public Slider slider;
        public TMP_InputField valueText;
        
        public float minValue;
        public float maxValue;
        public string formatString;

        protected override void Awake()
        {
            base.Awake();
            
            if (!slider)
                return;
            
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            valueText.onEndEdit.AddListener(OnTextEdited);
            valueText.onSubmit.AddListener(OnTextEdited);
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
            
            SubmitValue(newValue);
        }

        protected override void OnUpdateControls()
        {
            base.OnUpdateControls();

            float num = Mathf.Clamp(GetCurrentValue(), minValue, maxValue);

            if (slider)
                slider.value = num;
            
            //valueText.text

            if (valueText)
                valueText.text = string.Format(CultureInfo.InvariantCulture, formatString, num);
        }

        private void OnTextEdited(string newText)
        {
            if (float.TryParse(newText, out float num))
            {
                num = Mathf.Clamp(num, minValue, maxValue);
                
                SubmitValue(num);
            }
            else
            {
                SubmitValue(GetCurrentValue());
            }
        }

        public void MoveSlider(float delta)
        {
            if (slider)
                slider.normalizedValue += delta;
        }
    }
}