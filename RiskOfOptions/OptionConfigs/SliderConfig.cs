using System.Globalization;

namespace RiskOfOptions.OptionConfigs;

public class SliderConfig : BaseOptionConfig
{
    public float min = 0;
    public float max = 100;
    public string formatString = "{0:0}%";
        
    public SliderTryParse? TryParseDelegate { get; set; }

    public delegate bool SliderTryParse(string input, CultureInfo cultureInfo, out float value);
}