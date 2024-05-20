using System;
using System.Globalization;
using RiskOfOptions.Options;
using RoR2.UI;
using TMPro;

namespace RiskOfOptions.Components.Options;

public abstract class ModSettingsNumericField<TNumeric> : ModSettingsControl<TNumeric>
    where TNumeric : struct, IComparable, IComparable<TNumeric>, IConvertible, IEquatable<TNumeric>, IFormattable
{
    public TMP_InputField valueText;
    public TNumeric min;
    public TNumeric max;
    public string formatString;

    protected override void Awake()
    {
        base.Awake();
        
        valueText.onEndEdit.AddListener(OnTextEdited);
        valueText.onSubmit.AddListener(OnTextEdited);
    }

    protected override void Disable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = false;
    }

    protected override void Enable()
    {
        foreach (var button in GetComponentsInChildren<HGButton>())
            button.interactable = true;
    }
    
    protected override void OnUpdateControls()
    {
        base.OnUpdateControls();

        var num = Clamp(GetCurrentValue());
            
        if (valueText)
            valueText.text = string.Format(Separator.GetCultureInfo(), formatString, num);
    }

    private void OnTextEdited(string newText)
    {
        SubmitValue(TryParse(newText, NumberStyles.Any, Separator.GetCultureInfo(), out var num)
            ? Clamp(num)
            : GetCurrentValue());
    }

    protected abstract TNumeric Clamp(TNumeric value);

    protected abstract bool TryParse(string text, NumberStyles styles, IFormatProvider provider, out TNumeric num);
}