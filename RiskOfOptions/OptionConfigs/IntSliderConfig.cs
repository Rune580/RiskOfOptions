using System.Globalization;

namespace RiskOfOptions.OptionConfigs;

public class IntSliderConfig : BaseOptionConfig
{
    public int min = 0;
    public int max = 100;
    public string formatString = "{0}";
        
    public IntSliderTryParse? TryParseDelegate { get; set; }
        
    public delegate bool IntSliderTryParse(string input, CultureInfo cultureInfo, out int value);
}