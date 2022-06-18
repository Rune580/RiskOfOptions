using System;
using TMPro;

namespace RiskOfOptions.Components.Options;

public class ModSettingsNumericField<TNumeric> : ModSettingsControl<TNumeric>
    where TNumeric : struct, IComparable, IComparable<TNumeric>, IConvertible, IEquatable<TNumeric>, IFormattable
{
    public TMP_InputField valueText;
    public TNumeric min;
    public TNumeric max;
    public string formatString;

    protected override void Disable()
    {
        
    }

    protected override void Enable()
    {
        
    }
}