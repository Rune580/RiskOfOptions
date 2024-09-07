using System;
using System.Globalization;

namespace RiskOfOptions.OptionConfigs;

public class SliderConfig : BaseOptionConfig
{
    public float min = 0;
    public float max = 100;
    [Obsolete("Use FormatString Property instead")]
    public string formatString = "{0:0}%";

    /// <summary>
    /// Determines how the value is formatted in the GUI
    /// <remarks>https://learn.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting</remarks>
    /// </summary>
    public virtual string FormatString
    {
        get => formatString;
        set => formatString = value;
    }
        
    public SliderTryParse? TryParseDelegate { get; set; }

    public delegate bool SliderTryParse(string input, CultureInfo cultureInfo, out float value);
}