using System;
using System.Globalization;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Options;

public class ModSettingsStepSlider : ModSettingsControl<float, StepSliderConfig>
{
    public Slider slider;
    public TMP_InputField valueText;
        
    public float minValue;
    public float maxValue;
    public float increment;
    public bool remapManualInputToStep;
    public string formatString;
        
    private SliderConfig.SliderTryParse? _tryParse;

    protected override void Awake()
    {
        base.Awake();
            
        if (!slider)
            return;
            
        _tryParse = Config?.TryParseDelegate;
            
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
            valueText.text = string.Format(Separator.GetCultureInfo(), formatString, num);
    }
        
    private void OnTextEdited(string newText)
    {
        if (TryParse(newText, Separator.GetCultureInfo(), out var num))
        {
            num = Mathf.Clamp(num, minValue, maxValue);

            if (remapManualInputToStep)
            {
                float step = Mathf.Abs(num - minValue) / increment;
                num = Mathf.RoundToInt(step) * increment + minValue;
            }

            SubmitValue(num);
        }
        else
        {
            SubmitValue(GetCurrentValue());
        }
    }
        
    private bool TryParse(string input, CultureInfo cultureInfo, out float value)
    {
        return _tryParse is not null
            ? _tryParse(input, cultureInfo, out value)
            : float.TryParse(input, NumberStyles.Any, cultureInfo, out value);
    }

    public void MoveSlider(float delta)
    {
        if (slider)
            slider.normalizedValue += delta;
    }
}