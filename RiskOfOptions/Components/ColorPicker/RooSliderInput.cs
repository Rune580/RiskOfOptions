using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RiskOfOptions.Components.ColorPicker
{
    public class RooSliderInput : MonoBehaviour
    {
        public Slider slider;
        public TMP_InputField inputField;
        public string formatString = "{0}";

        public float sliderMin;
        public float sliderMax;
        public float valueMin;
        public float valueMax;

        public SliderInputEvent onValueChanged = new();

        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_value != value)
                {
                    _value = value;
                    onValueChanged.Invoke(_value);
                }
                
                UpdateControls();
            }
        }

        public void SetUnmappedValue(float value)
        {
            Value = value.Remap(sliderMin, sliderMax, valueMin, valueMax);
        }

        public void Awake()
        {
            slider.minValue = sliderMin;
            slider.maxValue = sliderMax;
            
            slider.onValueChanged.AddListener(SliderChanged);
            inputField.onEndEdit.AddListener(OnTextEdited);
            inputField.onSubmit.AddListener(OnTextEdited);
        }

        private void UpdateControls()
        {
            int remappedVal = (int)Value.Remap(valueMin, valueMax, sliderMin, sliderMax);
            
            slider.value = remappedVal;
            inputField.text = string.Format(CultureInfo.InvariantCulture, formatString, remappedVal);
        }

        private void SliderChanged(float newValue)
        {
            Value = newValue.Remap(sliderMin, sliderMax, valueMin, valueMax);
        }

        private void OnTextEdited(string newText)
        {
            if (float.TryParse(newText, out float num))
            {
                num = Mathf.Clamp(num, slider.minValue, slider.maxValue);

                Value = num.Remap(sliderMin, sliderMax, valueMin, valueMax);
            }
            else
            {
                Value = _value;
            }
        }
        
        [Serializable]
        public class SliderInputEvent : UnityEvent<float>
        {
            
        }
    }
}