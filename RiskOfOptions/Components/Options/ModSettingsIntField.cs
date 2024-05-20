using System;
using System.Globalization;
using RiskOfOptions.Options;
using UnityEngine;

namespace RiskOfOptions.Components.Options;

public class ModSettingsIntField : ModSettingsNumericField<int>
{
    protected override int Clamp(int value) => Mathf.Clamp(value, min, max);

    protected override bool TryParse(string text, NumberStyles styles, IFormatProvider provider, out int num) =>
        int.TryParse(text, NumberStyles.Any, Separator.GetCultureInfo(), out num);
}