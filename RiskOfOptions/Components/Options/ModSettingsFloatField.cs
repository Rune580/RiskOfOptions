using System;
using System.Globalization;
using UnityEngine;

namespace RiskOfOptions.Components.Options;

public class ModSettingsFloatField : ModSettingsNumericField<float>
{
    protected override float Clamp(float value) => Mathf.Clamp(value, min, max);

    protected override bool TryParse(string text, NumberStyles styles, IFormatProvider provider, out float num) =>
        float.TryParse(text, styles, provider, out num);
}